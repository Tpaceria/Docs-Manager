using Docs_Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docs_Manager.Data
{
    public partial class DatabaseService
    {

        public Task<List<EducationInfo>> GetEducationAsync()
        {
            return _database
                .Table<EducationInfo>()
                .ToListAsync();
        }

        public Task<int> SaveEducationAsync(
            EducationInfo education)
        {
            if (education.Id != 0)
            {
                return _database
                    .UpdateAsync(education);
            }

            return _database
                .InsertAsync(education);
        }

        public Task<int> DeleteEducationAsync(
            EducationInfo education)
        {
            return _database
                .DeleteAsync(education);
        }

    }
}
