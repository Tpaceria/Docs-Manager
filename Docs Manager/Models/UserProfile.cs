using SQLite;

namespace Docs_Manager.Models;

[Table("UserProfile")]
public class UserProfile
{
    [PrimaryKey]
    public int Id { get; set; } = 1;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }

    public DateTime BirthDate { get; set; }
}