using PayPhoneApiChallenge.App.Transactions.DTOs;

namespace PayPhoneApiChallenge.App.Transactions.Interfaces;

public interface ITransactionsService
{
    Task<IEnumerable<TransactionDto>> GetAllAsync();
    Task<TransactionDto> CreateAsync(CreateTransactionDto dto);
}