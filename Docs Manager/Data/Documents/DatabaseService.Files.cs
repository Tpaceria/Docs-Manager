using Docs_Manager.Models;

namespace Docs_Manager.Data;

public partial class DatabaseService
{
    public Task<List<StoredFile>> GetStoredFilesAsync()
    {
        return _database
            .Table<StoredFile>()
            .ToListAsync();
    }

    public async Task<StoredFile?> GetStoredFileAsync(
        int fileId)
    {
        return await _database
            .Table<StoredFile>()
            .Where(x => x.Id == fileId)
            .FirstOrDefaultAsync();
    }

    public Task<int> SaveStoredFileAsync(
        StoredFile file)
    {
        if (file.Id != 0)
        {
            return _database
                .UpdateAsync(file);
        }

        return _database
            .InsertAsync(file);
    }

    public Task<int> DeleteStoredFileAsync(
        StoredFile file)
    {
        return _database
            .DeleteAsync(file);
    }
}