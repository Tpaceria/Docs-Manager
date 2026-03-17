namespace Docs_Manager.Models
{
    public class AddFileModel
    {
        public string Category { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}