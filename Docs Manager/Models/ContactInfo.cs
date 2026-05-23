using SQLite;

namespace Docs_Manager.Models;

[Table("ContactInfo")]
public class ContactInfo
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int UserProfileId { get; set; }

    public string? ContactType { get; set; }
    // phone / email

    public string? Value { get; set; }

    public bool IsPrimary { get; set; }

    public bool WhatsApp { get; set; }

    public bool Telegram { get; set; }

    public bool Viber { get; set; }
}