using Docs_Manager.Models;

namespace Docs_Manager.Data;

public partial class DatabaseService
{
    /// <summary>
    /// Метод регистрирующий файл в FileManager в момент прикрепления к сертификату
    /// </summary>
    public async Task<FileRecord> RegisterAttachedFileAsync(string filePath, string certificateCategory, string certificateName)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        var fileInfo = new FileInfo(filePath);

        // Маппинг категорий сертификатов на категории FileManager
        var categoryMap = new Dictionary<string, string>
        {
            { "CERTIFICATES", "Сертификаты" },
            { "COC", "Опыт" },
            { "DOCUMENTS", "Документы" },
            { "MEDICINE", "Медицина" },
            { "OTHER", "Другое" }
        };

        var fileCategory = categoryMap.ContainsKey(certificateCategory)
            ? categoryMap[certificateCategory]
            : "Другое";

        // Копирование файла в папку FileManager
        var appDataPath = FileSystem.CacheDirectory;
        var categoryFolder = Path.Combine(appDataPath, fileCategory);

        if (!Directory.Exists(categoryFolder))
            Directory.CreateDirectory(categoryFolder);

        var extension = fileInfo.Extension;
        var formattedName = $"{certificateName}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
        var newFilePath = Path.Combine(categoryFolder, formattedName);

        // Копирование файла
        File.Copy(filePath, newFilePath, overwrite: true);

        // Не сохраняем новые пути, они находятся на темпоральных сайтах
        // Оставляем оригинальные пути в Certificate.FilePath
        var fileRecord = new FileRecord
        {
            Category = fileCategory,
            OriginalFileName = fileInfo.Name,
            FormattedFileName = formattedName,
            FilePath = newFilePath,
            FileSize = fileInfo.Length / 1024, // KB
            FileExtension = extension,
            DateAdded = DateTime.Now,
            Description = $"Прикреплен к: {certificateName}"
        };

        await SaveFileRecordAsync(fileRecord);

        return fileRecord;
    }
}
