using Microsoft.EntityFrameworkCore;

using PayPhoneApiChallenge.Domain.Transactions.Interfaces;
using PayPhoneApiChallenge.Domain.Transactions.Entities;

using PayPhoneApiChallenge.Infra.Persistence;

namespace PayPhoneApiChallenge.Infra.Repositories;


public class TransactionRepository: ITransactionRepository
{
    private readonly PayPhoneDbContext _context;

    public TransactionRepository(PayPhoneDbContext context)
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
            .Include(t => t.FromWallet)
            .Include(t => t.ToWallet)
            .ToListAsync();
    }

    public async Task<Transaction> GetTransactionById(int id)
    {
        return await _context.Transactions
            .Include(t => t.FromWallet)
            .Include(t => t.ToWallet)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}
