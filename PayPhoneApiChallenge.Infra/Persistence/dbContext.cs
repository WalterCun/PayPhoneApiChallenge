using Microsoft.EntityFrameworkCore;

using PayPhoneApiChallenge.Domain.Transactions.Entities;
using PayPhoneApiChallenge.Domain.Wallets.Entities;

namespace PayPhoneApiChallenge.Infra.Persistence
{
public class dbContext : DbContext { 
    public dbContext(DbContextOptions<dbContext> options) : base(options) { }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries()
        .Where(e => (e.Entity is Wallet || e.Entity is Transaction) && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
                var now = DateTime.UtcNow;

                if (entry.Entity is Wallet wallet)
                {
                    if (entry.State == EntityState.Added)
                        wallet.CreatedAt = now;

                    wallet.UpdatedAt = now;
                }
                else if (entry.Entity is Transaction transaction)
                {
                    if (entry.State == EntityState.Added)
                        transaction.CreatedAt = now;

                    transaction.UpdatedAt = now;
                }
            }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
             .HasOne(t => t.SenderWallet)
             .WithMany(w => w.TransactionsSent)
             .HasForeignKey(t => t.SenderWalletId)
             .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ReceiverWallet)
            .WithMany(w => w.TransactionsReceived)
            .HasForeignKey(t => t.ReceiverWalletId)
            .OnDelete(DeleteBehavior.Restrict);
    }



}
}