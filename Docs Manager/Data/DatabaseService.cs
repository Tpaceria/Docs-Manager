using SQLite;
using Docs_Manager.Models;

namespace Docs_Manager.Data;

public partial class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        var dbPath =
            Path.Combine(
                FileSystem.AppDataDirectory,
                "documents.db");

        _database =
            new SQLiteAsyncConnection(dbPath);

        _database.CreateTableAsync<Document>().Wait();
        _database.CreateTableAsync<UserProfile>().Wait();
        _database.CreateTableAsync<Certificate>().Wait();
        _database.CreateTableAsync<StoredFile>().Wait();
        _database.CreateTableAsync<Experience>().Wait();
        _database.CreateTableAsync<ContactInfo>().Wait();
        _database.CreateTableAsync<EducationInfo>().Wait();
        _database.CreateTableAsync<VisaModel>().Wait();
        _database.CreateTableAsync<SkillsModel>().Wait();
        _database.CreateTableAsync<BiometricModel>().Wait();
    }
}