using SQLite;

namespace Docs_Manager.Models;

[Table("Certificates")]
public class Certificate
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Document { get; set; }      // Название документа
    public string? Country { get; set; }       // Страна выдачи
    public string? Number { get; set; }        // Номер документа
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsLifetime { get; set; }
    public string? FilePath { get; set; }      // Путь к файлу

    // ➕ НОВОЕ ПОЛЕ
    public string Category { get; set; } = "CERTIFICATES";  // Категория: CERTIFICATES, COC & ENDORSEMENT, DOCUMENTS, MEDICINE, OTHER
}