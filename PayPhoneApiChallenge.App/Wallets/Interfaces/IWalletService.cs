using PayPhoneApiChallenge.App.Wallets.DTOs;

namespace PayPhoneApiChallenge.App.Wallets.Interfaces;

public interface IWalletService
{
    Task<IEnumerable<WalletDto>> GetAllAsync();
    Task<WalletDto> GetByIdAsync(int id);
    Task<WalletDto> CreateAsync(CreateWalletDto dto);
    Task<WalletDto> UpdateAsync(int id, UpdateWalletDto dto);
    Task<bool> DeleteAsync(int id);
}