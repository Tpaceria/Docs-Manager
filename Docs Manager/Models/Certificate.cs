using SQLite;

namespace Docs_Manager.Models;

[Table("Certificates")]
public class Certificate
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Number { get; set; }

    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    public bool IsLifetime { get; set; }
}