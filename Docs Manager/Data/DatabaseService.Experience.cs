using Docs_Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docs_Manager.Data
{
    public partial class DatabaseService
    {

        public async Task<List<Experience>> GetExperiencesAsync()
        {
            return await _database
                .Table<Experience>()
                .ToListAsync();
        }

        public async Task<Experience?> GetExperienceAsync(
            int id)
        {
            return await _database
                .Table<Experience>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveExperienceAsync(
            Experience experience)
        {
            if (experience.Id == 0)
            {
                return await _database
                    .InsertAsync(experience);
            }

            return await _database
                .UpdateAsync(experience);
        }

        public async Task<int> DeleteExperienceAsync(
            Experience experience)
        {
            return await _database
                .DeleteAsync(experience);
        }

    }
}
