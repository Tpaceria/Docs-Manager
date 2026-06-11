using SQLite;

namespace Docs_Manager.Models;

public class VisaModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Type { get; set; } = "";

    public string Country { get; set; } = "";

    public DateTime ExpiryDate { get; set; }
}