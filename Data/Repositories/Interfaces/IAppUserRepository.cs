using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IAppUserRepository
    {
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser?> GetByUserNameAsync(string userName);
        Task<bool> IsValidIdAsync(int id);
        Task<bool> IsValidUserNameAsync(string userName);

        Task<IEnumerable<AppUser>?> GetAllAsync();
        
        
    }
}
