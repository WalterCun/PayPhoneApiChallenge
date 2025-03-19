using System;
using PayPhoneApiChallenge.Domain.Wallets.Entities;

namespace PayPhoneApiChallenge.Domain.Transactions.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public int SenderWalletId { get; set; }  // add Control
        public int ReceiverWalletId { get; set; }  // add Control

        public decimal Amount { get; set; } 

        public TransactionType Type { get; set; }  

        public DateTime CreatedAt { get; set; }  
        public DateTime UpdatedAt { get; set; }  

        // Propiedades de navegación
        public Wallet SenderWallet { get; set; }
        public Wallet ReceiverWallet { get; set; }

    }
}
