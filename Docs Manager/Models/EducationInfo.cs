using SQLite;

namespace Docs_Manager.Models;

[Table("EducationInfo")]
public class EducationInfo
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Country { get; set; } = "";

    [MaxLength(100)]
    public string InstitutionType { get; set; } = "";

    [MaxLength(200)]
    public string Institution { get; set; } = "";

    [MaxLength(100)]
    public string Qualification { get; set; } = "";

    [MaxLength(100)]

    public DateTime GraduationDate { get; set; }

    public string? FilePath { get; set; }
}