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

    [Ignore]
    public int DaysUntilExpiry => IsLifetime ? int.MaxValue : (ExpiryDate - DateTime.Today).Days;

    [Ignore]
    public bool IsExpired => !IsLifetime && ExpiryDate < DateTime.Today;

    [Ignore]
    public bool IsExpiringSoon => !IsLifetime && !IsExpired && DaysUntilExpiry <= ExpiringSoonThresholdDays;

    private const int ExpiringSoonThresholdDays = 30;

    [Ignore]
    public string StatusDisplay
    {
        get
        {
            if (IsLifetime) return "Lifetime";
            if (IsExpired) return "Expired";
            if (IsExpiringSoon) return "Expiring Soon";
            return "Active";
        }
    }

    [Ignore]
    public string StatusColor
    {
        get
        {
            if (IsLifetime) return "#00d4ff";
            if (IsExpired) return "#DC3545";
            if (IsExpiringSoon) return "#FFC107";
            return "#28A745";
        }
    }

    [Ignore]
    public string DaysRemainingDisplay
    {
        get
        {
            if (IsLifetime) return "No expiration";
            int days = DaysUntilExpiry;
            if (days < 0) return $"Expired {-days} day{(-days == 1 ? "" : "s")} ago";
            if (days == 0) return "Expires today";
            return $"{days} day{(days == 1 ? "" : "s")} remaining";
        }
    }
}