using Docs_Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docs_Manager.Data
{
    public partial class DatabaseService
    {
        public Task<List<ContactInfo>> GetContactsAsync()
        {
            return _database
                .Table<ContactInfo>()
                .ToListAsync();
        }

        public Task<int> SaveContactAsync(
            ContactInfo contact)
        {
            if (contact.Id != 0)
            {
                return _database
                    .UpdateAsync(contact);
            }

            return _database
                .InsertAsync(contact);
        }

        public Task<int> DeleteContactAsync(
            ContactInfo contact)
        {
            return _database
                .DeleteAsync(contact);
        }

    }
}
