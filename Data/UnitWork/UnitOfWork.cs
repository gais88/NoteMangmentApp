using Core.Models;
using Data.Context;
using Data.Repositories.Interfaces;
using Data.Repositories.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public UnitOfWork(AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public IAppUserRepository AppUserRepository => new AppUserRepository(
           _dbContext, new Logger<AppUserRepository>(new NullLoggerFactory()));
        public INoteRepository NoteRepository => new NoteRepository(
            _dbContext, new Logger<NoteRepository>(new NullLoggerFactory()));

       

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
