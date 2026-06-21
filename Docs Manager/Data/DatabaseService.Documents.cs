using Docs_Manager.Models;

namespace Docs_Manager.Data;

public partial class DatabaseService
{
    public Task<List<Document>> GetDocumentsAsync()
    {
        return _database
            .Table<Document>()
            .ToListAsync();
    }

    public Task<int> SaveDocumentAsync(
        Document document)
    {
        if (document.Id != 0)
        {
            return _database
                .UpdateAsync(document);
        }

        return _database
            .InsertAsync(document);
    }

    public Task<int> DeleteDocumentAsync(
        Document document)
    {
        return _database
            .DeleteAsync(document);
    }
}