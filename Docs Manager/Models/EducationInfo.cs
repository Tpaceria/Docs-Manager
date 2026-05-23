using SQLite;

namespace Docs_Manager.Models;

[Table("EducationInfo")]
public class EducationInfo
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Qualification { get; set; }

    public string Institution { get; set; }

    public DateTime GraduationDate { get; set; }
}