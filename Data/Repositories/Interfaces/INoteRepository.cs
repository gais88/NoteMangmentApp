using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface INoteRepository
    {
        Task<Note> GetByIdAsync(int id);
        Task<IEnumerable<Note>?> GetAllAsync(int userId);
        Task AddAsync(Note? note);
        void Update(Note? note);
        void Delete(Note? note);
    }
}
