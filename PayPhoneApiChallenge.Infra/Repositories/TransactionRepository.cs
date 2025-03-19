using Microsoft.EntityFrameworkCore;

using PayPhoneApiChallenge.Domain.Transactions.Interfaces;
using PayPhoneApiChallenge.Domain.Transactions.Entities;

using PayPhoneApiChallenge.Infra.Persistence;

namespace PayPhoneApiChallenge.Infra.Repositories;


public class TransactionRepository: ITransactionRepository
{
    private readonly dbContext _context;

    public TransactionRepository(dbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await _context.Transactions
            .Include(t => t.SenderWallet)
            .Include(t => t.ReceiverWallet)
            .ToListAsync();
    }

    public async Task<Transaction> GetTransactionById(int id)
    {
        return await _context.Transactions
            .Include(t => t.SenderWallet)
            .Include(t => t.ReceiverWallet)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}
