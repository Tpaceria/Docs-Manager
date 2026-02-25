using SQLite;
using System;

namespace Docs_Manager.Models
{
    [SQLite.Table("Documents")]
    public class Document
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [SQLite.MaxLength(200)]
        public string? Title { get; set; }

        public string? Type { get; set; }

        public string? Number { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string? FilePath { get; set; }

        [Ignore]
        public bool IsExpired => ExpiryDate < DateTime.Today;

        [Ignore]
        public int DaysLeft => (ExpiryDate - DateTime.Today).Days;
    }
}