using SQLite;
using System;

namespace Docs_Manager.Models
{
    [Table("StoredFiles")]
    public class StoredFile
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(200)]
        public string FileName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Category { get; set; } = "other";

        public string FilePath { get; set; } = string.Empty;

        public long FileSizeBytes { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;

        public string FileType { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty; // ✅ ДОБАВЛЕНО

        [Ignore]
        public string FileSizeDisplay
        {
            get
            {
                return FormatFileSize(FileSizeBytes);
            }
        }

        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}