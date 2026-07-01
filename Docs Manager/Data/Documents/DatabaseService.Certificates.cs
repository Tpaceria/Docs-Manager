using Docs_Manager.Models;

namespace Docs_Manager.Data;

public partial class DatabaseService
{
    public Task<List<Certificate>> GetCertificatesAsync()
    {
        return _database
            .Table<Certificate>()
            .ToListAsync();
    }

    public Task<int> SaveCertificateAsync(
        Certificate certificate)
    {
        if (certificate.Id != 0)
        {
            return _database
                .UpdateAsync(certificate);
        }

        return _database
            .InsertAsync(certificate);
    }

    public Task<int> DeleteCertificateAsync(
        Certificate certificate)
    {
        return _database
            .DeleteAsync(certificate);
    }
}