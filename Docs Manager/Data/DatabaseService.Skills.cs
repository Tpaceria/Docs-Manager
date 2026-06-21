using Docs_Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docs_Manager.Data
{
    public partial class DatabaseService
    {

        public Task<List<SkillsModel>> GetSkillsAsync()
        {
            return _database
                .Table<SkillsModel>()
                .ToListAsync();
        }

        public Task<int> SaveSkillsAsync(
            SkillsModel skills)
        {
            if (skills.Id != 0)
            {
                return _database.UpdateAsync(skills);
            }

            return _database.InsertAsync(skills);
        }

        public Task<int> DeleteSkillsAsync(
            SkillsModel skills)
        {
            return _database.DeleteAsync(skills);
        }

    }
}
