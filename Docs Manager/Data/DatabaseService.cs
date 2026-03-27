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

            // Создаём таблицы (убрал дублирование StoredFile)
            _database.CreateTableAsync<Document>().Wait();
            _database.CreateTableAsync<UserProfile>().Wait();
            _database.CreateTableAsync<Certificate>().Wait();
            _database.CreateTableAsync<StoredFile>().Wait();
            _database.CreateTableAsync<Experience>().Wait();
            _database.CreateTableAsync<Experience>().Wait();  // ← ДОБАВЬ ЭТУ СТРОКУ
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

        // -------------------------
        // STORED FILES / FILES
        // -------------------------

        public Task<List<StoredFile>> GetStoredFilesAsync()
        {
            return _database.Table<StoredFile>().ToListAsync();
        }

        public async Task<StoredFile?> GetStoredFileAsync(int fileId)
        {
            return await _database.Table<StoredFile>()
                                  .Where(x => x.Id == fileId)
                                  .FirstOrDefaultAsync();
        }

        public Task<int> SaveStoredFileAsync(StoredFile file)
        {
            if (file.Id != 0)
                return _database.UpdateAsync(file);
            else
                return _database.InsertAsync(file);
        }

        public Task<int> DeleteStoredFileAsync(StoredFile file)
        {
            return _database.DeleteAsync(file);
        }

        // Alias методы для удобства
        public Task<List<StoredFile>> GetFilesByCategoryAsync(string category)
        {
            return _database.Table<StoredFile>()
                .Where(f => f.Category == category)
                .ToListAsync();
        }

        public Task<List<StoredFile>> GetAllFilesAsync()
        {
            return _database.Table<StoredFile>().ToListAsync();
        }

        public Task<int> SaveFileAsync(StoredFile file)
        {
            return SaveStoredFileAsync(file);
        }

        public Task<int> DeleteFileAsync(StoredFile file)
        {
            return DeleteStoredFileAsync(file);
        }

        // -------------------------
        // EXPERIENCE
        // -------------------------

        public async Task<List<Experience>> GetExperiencesAsync()
        {
            return await _database.Table<Experience>().ToListAsync();
        }

        public async Task<Experience?> GetExperienceAsync(int id)
        {
            return await _database.Table<Experience>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveExperienceAsync(Experience experience)
        {
            if (experience.Id == 0)
                return await _database.InsertAsync(experience);
            else
                return await _database.UpdateAsync(experience);
        }

        public async Task<int> DeleteExperienceAsync(Experience experience)
        {
            return await _database.DeleteAsync(experience);
        }
    }
}