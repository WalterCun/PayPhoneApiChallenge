using PayPhoneApiChallenge.Domain.Transactions.Entities;

namespace PayPhoneApiChallenge.Domain.Transactions.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetTransactionById(int id);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task AddAsync(Transaction transaction);
    }
}
