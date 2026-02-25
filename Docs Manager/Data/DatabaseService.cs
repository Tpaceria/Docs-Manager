using SQLite;
using Docs_Manager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace Docs_Manager.Data
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "documents.db");
            _database = new SQLiteAsyncConnection(dbPath);

            // Создаём таблицы
            _database.CreateTableAsync<Document>().Wait();
            _database.CreateTableAsync<UserProfile>().Wait();
            _database.CreateTableAsync<Certificate>().Wait();   // ← ВОТ ЭТОГО НЕ ХВАТАЛО
        }
        // -------------------------
        // DOCUMENTS
        // -------------------------

        public Task<List<Document>> GetDocumentsAsync()
        {
            return _database.Table<Document>().ToListAsync();
        }

        public Task<int> SaveDocumentAsync(Document document)
        {
            if (document.Id != 0)
                return _database.UpdateAsync(document);
            else
                return _database.InsertAsync(document);
        }

        public Task<int> DeleteDocumentAsync(Document document)
        {
            return _database.DeleteAsync(document);
        }

        // -------------------------
        // USER PROFILE
        // -------------------------

        public async Task<UserProfile?> GetUserProfileAsync()
        {
            return await _database.Table<UserProfile>()
                                  .Where(x => x.Id == 1)
                                  .FirstOrDefaultAsync();
        }

        public async Task SaveUserProfileAsync(UserProfile profile)
        {
            var existing = await GetUserProfileAsync();

            if (existing == null)
                await _database.InsertAsync(profile);
            else
                await _database.UpdateAsync(profile);
        }

        // -------------------------
        // CERTIFICATES
        // -------------------------

        public Task<List<Certificate>> GetCertificatesAsync()
        {
            return _database.Table<Certificate>().ToListAsync();
        }

        public Task<int> SaveCertificateAsync(Certificate certificate)
        {
            if (certificate.Id != 0)
                return _database.UpdateAsync(certificate);
            else
                return _database.InsertAsync(certificate);
        }

        public Task<int> DeleteCertificateAsync(Certificate certificate)
        {
            return _database.DeleteAsync(certificate);
        }

    }
}