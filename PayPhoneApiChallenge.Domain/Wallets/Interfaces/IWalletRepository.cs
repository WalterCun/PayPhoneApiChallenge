using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPhoneApiChallenge.Domain.Wallets.Interfaces
{
    public interface IWalletRepository
    {
        Task<Entities.Wallet> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Wallet>> GetAllAsync();
        Task AddAsync(Entities.Wallet wallet);
        Task UpdateAsync(Entities.Wallet wallet);
        Task DeleteAsync(int id);
    }
}
