using Core.Models;
using Data.Context;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<AppUserRepository> _logger;
    

        public AppUserRepository(AppDbContext dbContext, ILogger<AppUserRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
           
        }

        public async Task<AppUser?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("GetByIdAsync for AppUser was Called");

                return await _dbContext.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to GetByIdAsync for AppUser: {ex.Message}");
                return null;
            }
        }
        public async Task<AppUser?> GetByUserNameAsync(string userName)
        {
            try
            {
                _logger.LogInformation("GetByUserNameAsync for AppUser was Called");

                return await _dbContext.AppUsers.FirstOrDefaultAsync(x => x.UserName!.ToLower() == userName.ToLower());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to GetByUserNameAsync for AppUser: {ex.Message}");
                return null;
            }
        }


        public async Task<bool> IsValidIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("IsValidIdAsync for AppUser was Called");
                return await _dbContext.AppUsers.AnyAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to IsValidIdAsync for AppUser: {ex.Message}");
                return true;
            }
        }
        public async Task<bool> IsValidUserNameAsync(string userName)
        {
            try
            {
                _logger.LogInformation("IsValidUserNameAsync for AppUser was Called");
                return await _dbContext.AppUsers.AnyAsync(x => x.UserName!.ToLower() == userName.ToLower());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to IsValidUserNameAsync for AppUser: {ex.Message}");
                return true;
            }
        }

        public async Task<IEnumerable<AppUser>?> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("GetAllAsync for AppUser was Called");

                return await _dbContext.AppUsers.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to GetAllAsync for AppUser: {ex.Message}");
                return null;
            }
        }
        public async Task<IEnumerable<AppUser>?> GetAllByRoleIdAsync(int roleId)
        {
            try
            {
                _logger.LogInformation("GetAllByRoleIdAsync for AppUser was Called");

                return await _dbContext.AppUsers.Where(x => x.AppRoles.Any(x => x.Id == roleId))
                                                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to GetAllByRoleIdAsync for AppUser: {ex.Message}");
                return null;
            }
        }
        public async Task<IEnumerable<AppUser>?> GetAllByRoleNameAsync(string roleName)
        {
            try
            {
                _logger.LogInformation("GetAllByRoleNameAsync for AppUser was Called");

                return await _dbContext.AppUsers.Where(x => x.AppRoles.Any(x => x.Name!.ToLower() == roleName.ToLower()))
                                                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to GetAllByRoleNameAsync for AppUser: {ex.Message}");
                return null;
            }
        }

        
    }
}
