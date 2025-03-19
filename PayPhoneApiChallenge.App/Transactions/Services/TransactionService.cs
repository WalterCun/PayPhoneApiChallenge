using Microsoft.EntityFrameworkCore;
using PayPhoneApiChallenge.App.Transactions.DTOs;
using PayPhoneApiChallenge.Domain.Transactions.Entities;
using PayPhoneApiChallenge.App.Transactions.Interfaces;
using PayPhoneApiChallenge.Infra.Persistence;

namespace PayPhoneApiChallenge.App.Transactions.Services;

public class TransactionService(PayPhoneDbContext context) : ITransactionsService
{
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

    public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
    {
        var fromWallet = await context.Wallets.FindAsync(dto.FromWalletId);
        var toWallet = await context.Wallets.FindAsync(dto.ToWalletId);

        if (fromWallet == null || toWallet == null)
            throw new Exception("Una o ambas billeteras no existen");

        if (fromWallet.Balance < dto.Amount)
            throw new Exception("Fondos insuficientes");

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            fromWallet.Balance -= dto.Amount;
            toWallet.Balance += dto.Amount;

            var transactionEntity = new Transaction
            {
                FromWalletId = dto.FromWalletId,
                ToWalletId = dto.ToWalletId,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            context.Transactions.Add(transactionEntity);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new TransactionDto
            {
                Id = transactionEntity.Id,
                FromWalletId = transactionEntity.FromWalletId,
                ToWalletId = transactionEntity.ToWalletId,
                Amount = transactionEntity.Amount,
                CreatedAt = transactionEntity.CreatedAt
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}