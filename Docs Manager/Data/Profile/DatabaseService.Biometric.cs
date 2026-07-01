using Docs_Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docs_Manager.Data
{
    public partial class DatabaseService
    {
        public Task<List<BiometricModel>> GetBiometricAsync()
        {
            return _database
                .Table<BiometricModel>()
                .ToListAsync();
        }

        public Task<int> SaveBiometricAsync(
            BiometricModel biometric)
        {
            if (biometric.Id != 0)
            {
                return _database.UpdateAsync(
                    biometric);
            }

            return _database.InsertAsync(
                biometric);
        }

        public Task<int> DeleteBiometricAsync(
            BiometricModel biometric)
        {
            return _database.DeleteAsync(
                biometric);
        }

    }
}
