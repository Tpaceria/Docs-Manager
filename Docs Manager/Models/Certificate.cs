using SQLite;

namespace Docs_Manager.Models;

[Table("certificates")]
public class Certificate
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Column("document")]
    public string Document { get; set; } = string.Empty;

    [Column("country")]
    public string? Country { get; set; }

    [Column("number")]
    public string Number { get; set; } = string.Empty;

    [Column("issue_date")]
    public DateTime IssueDate { get; set; }

    [Column("expiry_date")]
    public DateTime ExpiryDate { get; set; }

    [Column("is_lifetime")]
    public bool IsLifetime { get; set; }

    [Column("file_path")]
    public string? FilePath { get; set; }

    [Column("category")]
    public string Category { get; set; } = "CERTIFICATES";

    [Column("description")]
    public string? Description { get; set; }
}