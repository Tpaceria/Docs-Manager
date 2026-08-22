using Docs_Manager.Models;

namespace Docs_Manager.Data;

public partial class DatabaseService
{
    public Task<List<FileRecord>> GetAllFilesAsync()
    {
        return _database
            .Table<FileRecord>()
            .OrderByDescending(x => x.DateAdded)
            .ToListAsync();
    }

    public Task<List<FileRecord>> GetFilesByCategoryAsync(string category)
    {
        return _database
            .Table<FileRecord>()
            .Where(x => x.Category == category)
            .OrderByDescending(x => x.DateAdded)
            .ToListAsync();
    }

    public Task<int> SaveFileRecordAsync(FileRecord fileRecord)
    {
        if (fileRecord.Id != 0)
        {
            return _database.UpdateAsync(fileRecord);
        }

        return _database.InsertAsync(fileRecord);
    }

    public Task<int> DeleteFileRecordAsync(FileRecord fileRecord)
    {
        // Удалить физический файл
        if (File.Exists(fileRecord.FilePath))
        {
            try
            {
                File.Delete(fileRecord.FilePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting file: {ex.Message}");
            }
        }

        return _database.DeleteAsync(fileRecord);
    }
}
