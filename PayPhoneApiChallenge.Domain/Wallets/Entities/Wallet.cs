using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PayPhoneApiChallenge.Domain.Transactions.Entities;
using PayPhoneApiChallenge.Domain.Wallets.Entities;

namespace PayPhoneApiChallenge.Domain.Wallets.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public required string DocumentId { get; set; } 
        public required string Name { get; set; }
        public decimal Balance { get; set; } = 0;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Propiedades de navegación
        public ICollection<Transaction> TransactionsSent { get; set; } = new List<Transaction>();
        public ICollection<Transaction> TransactionsReceived { get; set; } = new List<Transaction>();
    }
}
