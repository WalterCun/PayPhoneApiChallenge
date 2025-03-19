using Microsoft.EntityFrameworkCore;
using PayPhoneApiChallenge.App.Transactions.DTOs;
using PayPhoneApiChallenge.Domain.Transactions.Entities;
using PayPhoneApiChallenge.App.Transactions.Interfaces;
using PayPhoneApiChallenge.Infra.Persistence;

namespace PayPhoneApiChallenge.App.Transactions.Services;

public class TransactionService(PayPhoneDbContext context) : ITransactionsService
{
    private async Task<TransactionDto> ProcessTransactionAsync(CreateTransactionDto dto)
    {
        // Validaciones de los datos de entrada
        if (dto.Amount <= 0)
            throw new ArgumentException("El monto debe ser mayor que 0.");

        if (dto.FromWalletId <= 0 || dto.ToWalletId <= 0)
            throw new ArgumentException("Los ID de billetera son obligatorios y deben ser mayores que 0.");

        // Buscar las billeteras en la base de datos
        var senderWallet = await context.Wallets.FindAsync(dto.FromWalletId);
        var receiverWallet = await context.Wallets.FindAsync(dto.ToWalletId);

        // Validaciones si las billeteras existen
        if (senderWallet == null || receiverWallet == null)
            throw new InvalidOperationException("Una o ambas billeteras no existen.");

        // Validación si la billetera de origen tiene suficiente saldo
        if (senderWallet.Balance < dto.Amount)
            throw new InvalidOperationException("Saldo insuficiente en la billetera de origen.");

        // Realizar la transferencia: Restar de la billetera de origen y sumar a la billetera de destino
        senderWallet.Balance -= dto.Amount;
        receiverWallet.Balance += dto.Amount;

        // Crear la transacción
        var transaction = new Transaction
        {
            FromWalletId = dto.FromWalletId,
            ToWalletId = dto.ToWalletId,
            Amount = dto.Amount,
            CreatedAt = DateTime.UtcNow
        };

        // Agregar la transacción al contexto y guardar los cambios
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        // Retornar la DTO de la transacción creada
        return new TransactionDto
        {
            Id = transaction.Id,
            FromWalletId = transaction.FromWalletId,
            ToWalletId = transaction.ToWalletId,
            Amount = transaction.Amount,
            CreatedAt = transaction.CreatedAt
        };
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
    {
        var providerName = context.Database.ProviderName;

        if (providerName == "Microsoft.EntityFrameworkCore.InMemory")
        {
            return await ProcessTransactionAsync(dto); // No usa transacción
        }

        await using var transaction = await context.Database.BeginTransactionAsync();
        var result = await ProcessTransactionAsync(dto);
        await transaction.CommitAsync();
        return result;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync()
    {
        return await context.Transactions
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                FromWalletId = t.FromWalletId,
                ToWalletId = t.ToWalletId,
                Amount = t.Amount,
                CreatedAt = t.CreatedAt
            }).ToListAsync();
    }
}