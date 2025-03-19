using Microsoft.EntityFrameworkCore;

using PayPhoneApiChallenge.Domain.Transactions.Entities;
using PayPhoneApiChallenge.Domain.Wallets.Entities;

namespace PayPhoneApiChallenge.Infra.Persistence
{
public class PayPhoneDbContext(DbContextOptions<PayPhoneDbContext> options) : DbContext(options)
{
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries()
        .Where(e => e.Entity is Wallet or Transaction && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow;

            switch (entry.Entity)
            {
                case Wallet wallet:
                {
                    if (entry.State == EntityState.Added)
                        wallet.CreatedAt = now;

                    wallet.UpdatedAt = now;
                    break;
                }
                case Transaction transaction:
                {
                    if (entry.State == EntityState.Added)
                        transaction.CreatedAt = now;

                    transaction.UpdatedAt = now;
                    break;
                }
            }
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.FromWallet)
            .WithMany(w => w.TransactionsSent)
            .HasForeignKey(t => t.FromWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ToWallet)
            .WithMany(w => w.TransactionsReceived)
            .HasForeignKey(t => t.ToWalletId)
            .OnDelete(DeleteBehavior.Restrict);
    }



}
}