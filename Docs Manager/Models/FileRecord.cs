namespace Docs_Manager.Models;

public class FileRecord
{
    [SQLite.PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Категория файла (Сертификаты, Документы, Другое)
    /// </summary>
    public string Category { get; set; } = "OTHER";

    /// <summary>
    /// Оригинальное имя файла
    /// </summary>
    public string OriginalFileName { get; set; } = "";

    /// <summary>
    /// Переименованное имя в едином формате
    /// </summary>
    public string FormattedFileName { get; set; } = "";

    /// <summary>
    /// Полный путь к файлу на устройстве
    /// </summary>
    public string FilePath { get; set; } = "";

    /// <summary>
    /// Размер файла в байтах
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Дата добавления файла
    /// </summary>
    public DateTime DateAdded { get; set; } = DateTime.Now;

    /// <summary>
    /// Расширение файла
    /// </summary>
    public string FileExtension { get; set; } = "";

    /// <summary>
    /// Описание или заметка к файлу
    /// </summary>
    public string Description { get; set; } = "";
}