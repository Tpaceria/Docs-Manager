using SQLite;

namespace Docs_Manager.Models;

[Table("file_records")]
public class FileRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Категория файла (Сертификаты, Документы, Другое)
    /// </summary>
    [Column("category")]
    public string Category { get; set; } = "OTHER";

    /// <summary>
    /// Оригинальное имя файла
    /// </summary>
    [Column("original_file_name")]
    public string OriginalFileName { get; set; } = "";

    /// <summary>
    /// Переименованное имя в едином формате
    /// </summary>
    [Column("formatted_file_name")]
    public string FormattedFileName { get; set; } = "";

    /// <summary>
    /// Полный путь к файлу на устройстве
    /// </summary>
    [Column("file_path")]
    public string FilePath { get; set; } = "";

    /// <summary>
    /// Размер файла в байтах
    /// </summary>
    [Column("file_size")]
    public long FileSize { get; set; }

    /// <summary>
    /// Дата добавления файла
    /// </summary>
    [Column("date_added")]
    public DateTime DateAdded { get; set; } = DateTime.Now;

    /// <summary>
    /// Расширение файла
    /// </summary>
    [Column("file_extension")]
    public string FileExtension { get; set; } = "";

    /// <summary>
    /// Описание или заметка к файлу
    /// </summary>
    [Column("description")]
    public string Description { get; set; } = "";
}