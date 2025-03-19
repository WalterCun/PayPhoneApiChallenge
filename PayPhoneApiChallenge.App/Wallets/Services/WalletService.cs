using Microsoft.EntityFrameworkCore;
using PayPhoneApiChallenge.App.Wallets.DTOs;
using PayPhoneApiChallenge.App.Wallets.Interfaces;
using PayPhoneApiChallenge.Domain.Wallets.Entities;
using PayPhoneApiChallenge.Infra.Persistence;

namespace PayPhoneApiChallenge.App.Wallets.Services;

public class WalletService(PayPhoneDbContext payPhoneDbContext) : IWalletService
{
    public async Task<IEnumerable<WalletDto>> GetAllAsync()
    {
        return await payPhoneDbContext.Wallets
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
        var wallet = await payPhoneDbContext.Wallets.FindAsync(id);
        if (wallet is null) return null;

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
        if (string.IsNullOrWhiteSpace(dto.DocumentId))
            throw new ArgumentException("El DocumentId es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("El Name es obligatorio.");

        // Verificar si ya existe una billetera con el mismo DocumentId
        var exists = await payPhoneDbContext.Wallets
            .AnyAsync(w => w.DocumentId == dto.DocumentId);
        if (exists)
            throw new InvalidOperationException("Ya existe una billetera con el DocumentId proporcionado.");

        // crear la nueva billetera
        var wallet = new Wallet
        {
            DocumentId = dto.DocumentId,
            Name = dto.Name,
            Balance = dto.Balance,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        payPhoneDbContext.Wallets.Add(wallet);
        await payPhoneDbContext.SaveChangesAsync();

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
        var wallet = await payPhoneDbContext.Wallets.FindAsync(id);
        if (wallet == null) return null;

        wallet.Name = dto.Name;
        wallet.Balance = dto.Balance;
        wallet.UpdatedAt = DateTime.UtcNow;

        await payPhoneDbContext.SaveChangesAsync();

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
        var wallet = await payPhoneDbContext.Wallets.FindAsync(id);
        if (wallet == null) return false;

        payPhoneDbContext.Wallets.Remove(wallet);
        await payPhoneDbContext.SaveChangesAsync();
        return true;
    }
}