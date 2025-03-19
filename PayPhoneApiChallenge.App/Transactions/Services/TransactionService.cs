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
        var senderWallet = await context.Wallets.FindAsync(dto.ToWalletId);
        var receiverWallet = await context.Wallets.FindAsync(dto.FromWalletId);

        if (senderWallet == null || receiverWallet == null)
            throw new InvalidOperationException("Wallet not found.");

        if (senderWallet.Balance < dto.Amount)
            throw new InvalidOperationException("Insufficient funds.");

        senderWallet.Balance -= dto.Amount;
        receiverWallet.Balance += dto.Amount;

        var transaction = new Transaction
        {
            ToWalletId = dto.ToWalletId,
            FromWalletId = dto.FromWalletId,
            Amount = dto.Amount,
            CreatedAt = DateTime.UtcNow
        };

        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        return new TransactionDto
        {
            Id = transaction.Id,
            ToWalletId = transaction.ToWalletId,
            FromWalletId = transaction.FromWalletId,
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

        using var transaction = await context.Database.BeginTransactionAsync();
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