using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitWork
{
    public interface IUnitOfWork
    {
        public IAppUserRepository AppUserRepository { get; }
        public INoteRepository NoteRepository { get; }
        Task<bool> SaveAsync();
    }
}
