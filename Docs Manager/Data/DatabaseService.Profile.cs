using Docs_Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docs_Manager.Data
{
    public partial class DatabaseService
    {

        public async Task<UserProfile?> GetUserProfileAsync()
        {
            return await _database
                .Table<UserProfile>()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync();
        }

        public async Task SaveUserProfileAsync(
            UserProfile profile)
        {
            var existing =
                await GetUserProfileAsync();

            if (existing == null)
            {
                await _database
                    .InsertAsync(profile);
            }
            else
            {
                await _database
                    .UpdateAsync(profile);
            }
        }

    }
}
