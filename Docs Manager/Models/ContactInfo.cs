using SQLite;

namespace Docs_Manager.Models;

[Table("ContactInfo")]
public class ContactInfo
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string ContactType { get; set; } = "";

    public string Value { get; set; } = "";

    public bool WhatsApp { get; set; }

    public bool Telegram { get; set; }

    public bool Viber { get; set; }
}