using SQLite;

namespace Docs_Manager.Models;

[Table("Documents")]
public class Document
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Passport, Seaman Book, Residence Permit,
    // National ID, Visa
    public string? Type { get; set; }

    // Ordinary Passport, USA C1/D,
    // Bulgarian Residence Permit...
    public string? Title { get; set; }

    public string? Number { get; set; }

    public string? Country { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool Lifetime { get; set; }

    // Issuing authority
    public string? IssuedBy { get; set; }

    // Free notes
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

    [Ignore]
    public string Status =>
        Lifetime
            ? "Lifetime"
            : IsExpired
                ? "Expired"
                : $"{DaysLeft} days";
}