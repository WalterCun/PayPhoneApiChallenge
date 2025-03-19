using Microsoft.EntityFrameworkCore;
using PayPhoneApiChallenge.Domain.Wallets.Entities;
using PayPhoneApiChallenge.Domain.Transactions.Entities;
using PayPhoneApiChallenge.Infra.Persistence;

public static class TestSeeder
{
    public static void Seed(PayPhoneDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        // Wallet 1
        Wallet wallet1 = new Wallet
        {
            DocumentId = "1",
            Name = "Walter",
            Balance = 1000
        };

        // Wallet 2
        Wallet wallet2 = new Wallet
        {
            DocumentId = "1",
            Name = "Rafael",
            Balance = 200
        };

        // Agregamos una transacción
        var transaction = new Transaction
        {
            FromWalletId = wallet1.Id,
            ToWalletId = wallet2.Id,
            Amount = 50,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            FromWallet = wallet1,
            ToWallet = wallet2
        };

        context.Transactions.Add(transaction);

        context.SaveChanges();
    }
}
