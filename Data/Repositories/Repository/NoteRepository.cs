using Core.Models;
using Data.Context;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<NoteRepository> _logger;

        public NoteRepository(AppDbContext dbContext,ILogger<NoteRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(Note? note)
        {
            try
            {
                _logger.LogInformation("AddAsync for note was Called");

                if (note != null)
                {


                    await _dbContext.Notes.AddAsync(note);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to AddAsync for note: {ex.Message}");
            }
        }

        public void Delete(Note? note)
        {
            try
            {
                _logger.LogInformation("Delete for note was Called");

                if (note != null)
                {
                    _dbContext.Notes.Remove(note);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to Delete for note: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Note>?> GetAllAsync(int userId)
        {
            try
            {
                _logger.LogInformation("GetAllAsync for Note was Called");

                return await _dbContext.Notes.Where(x=>x.AppUserId == userId)
                                             .AsNoTracking()
                                             .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to GetAllAsync for Note: {ex.Message}");
                return null;
            }
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("GetByIdAsync for Note was Called");

                return await _dbContext.Notes.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to GetByIdAsync for Note: {ex.Message}");
                return null;
            }
        }

        public void Update(Note? note)
        {
            try
            {
                _logger.LogInformation("Update for note was Called");
                if (note != null)
                {


                    _dbContext.Entry(note).State = EntityState.Modified;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to Update for note: {ex.Message}");
            }
        }
    }
}
