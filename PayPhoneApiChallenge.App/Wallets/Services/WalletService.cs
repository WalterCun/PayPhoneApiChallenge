using Microsoft.EntityFrameworkCore;
using PayPhoneApiChallenge.App.Wallets.DTOs;
using PayPhoneApiChallenge.App.Wallets.Interfaces;
using PayPhoneApiChallenge.Domain.Wallets.Entities;
using PayPhoneApiChallenge.Infra.Persistence;

namespace PayPhoneApiChallenge.App.Wallets.Services;

public class WalletService: IWalletService
{
    private readonly PayPhoneDbContext _context;
    
    public WalletService(PayPhoneDbContext payPhoneDbContext)
    {
        _context = payPhoneDbContext;
    }
    public async Task<IEnumerable<WalletDto>> GetAllAsync()
    {
        return await _context.Wallets
            .Select(w => new WalletDto
            {
                Id = w.Id,
                DocumentId = w.DocumentId,
                Name = w.Name,
                Balance = w.Balance,
                CreatedAt = w.CreatedAt,
                UpdatedAt = w.UpdatedAt
            }).ToListAsync();
    }
    public async Task<WalletDto> GetByIdAsync(int id)
    {
        var wallet = await _context.Wallets.FindAsync(id);
        if (wallet == null) return null;

        return new WalletDto
        {
            Id = wallet.Id,
            DocumentId = wallet.DocumentId,
            Name = wallet.Name,
            Balance = wallet.Balance,
            CreatedAt = wallet.CreatedAt,
            UpdatedAt = wallet.UpdatedAt
        };
    }
    public async Task<WalletDto> CreateAsync(CreateWalletDto dto)
    {
        var wallet = new Wallet
        {
            DocumentId = dto.DocumentId,
            Name = dto.Name,
            Balance = dto.Balance,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();

        return new WalletDto
        {
            Id = wallet.Id,
            DocumentId = wallet.DocumentId,
            Name = wallet.Name,
            Balance = wallet.Balance,
            CreatedAt = wallet.CreatedAt,
            UpdatedAt = wallet.UpdatedAt
        };
    }
    public async Task<WalletDto> UpdateAsync(int id, UpdateWalletDto dto)
    {
        var wallet = await _context.Wallets.FindAsync(id);
        if (wallet == null) return null;

        wallet.Name = dto.Name;
        wallet.Balance = dto.Balance;
        wallet.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new WalletDto
        {
            Id = wallet.Id,
            DocumentId = wallet.DocumentId,
            Name = wallet.Name,
            Balance = wallet.Balance,
            CreatedAt = wallet.CreatedAt,
            UpdatedAt = wallet.UpdatedAt
        };
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var wallet = await _context.Wallets.FindAsync(id);
        if (wallet == null) return false;

        _context.Wallets.Remove(wallet);
        await _context.SaveChangesAsync();
        return true;
    }
}