using Docs_Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docs_Manager.Data
{
    public partial class DatabaseService
    {

        public Task<List<VisaModel>> GetVisasAsync()
        {
            return _database
                .Table<VisaModel>()
                .ToListAsync();
        }

        public Task<int> SaveVisaAsync(
            VisaModel visa)
        {
            if (visa.Id != 0)
            {
                return _database
                    .UpdateAsync(visa);
            }

            return _database
                .InsertAsync(visa);
        }

        public Task<int> DeleteVisaAsync(
            VisaModel visa)
        {
            return _database
                .DeleteAsync(visa);
        }

    }
}
