using SQLite;

namespace Docs_Manager.Models;

[Table("Documents")]
public class Document
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Passport, Visa, Seaman Book, Residence Permit...
    public string? Type { get; set; }

    // Ordinary Passport, D-1, CDC и т.п.
    public string? Title { get; set; }

    public string? Number { get; set; }

    public string? Country { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool Lifetime { get; set; }

    public string? Notes { get; set; }

    public string? FilePath { get; set; }

    [Ignore]
    public bool IsExpired =>
        !Lifetime &&
        ExpiryDate < DateTime.Today;

    [Ignore]
    public int DaysLeft =>
        Lifetime
            ? int.MaxValue
            : (ExpiryDate - DateTime.Today).Days;
}