using Microsoft.EntityFrameworkCore;
using PayPhoneApiChallenge.Domain.Wallets.Entities;
using PayPhoneApiChallenge.Domain.Wallets.Interfaces;

using PayPhoneApiChallenge.Infra.Persistence;

namespace PayPhoneApiChallenge.Infra.Repositories;

public class WalletRepository : IWalletRepository
{ 
    private readonly dbContext _context;

    public WalletRepository(dbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var wallet = await _context.Wallets.FindAsync(id);
        if (wallet != null)
        {
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Wallet>> GetAllAsync()
    {
        return await _context.Wallets.ToListAsync();
    }

    public async Task<Wallet> GetByIdAsync(int id)
    {
        return await _context.Wallets.FindAsync(id);
    }


    public async Task UpdateAsync(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync();
    }

}
