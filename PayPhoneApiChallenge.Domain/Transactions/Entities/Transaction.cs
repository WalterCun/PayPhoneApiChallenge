using PayPhoneApiChallenge.Domain.Wallets.Entities;

namespace PayPhoneApiChallenge.Domain.Transactions.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public int FromWalletId { get; set; } // Relación
        public int ToWalletId { get; set; } // Relación

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Referenciaa
        public Wallet ToWallet { get; set; }
        public Wallet FromWallet { get; set; }
    }
}