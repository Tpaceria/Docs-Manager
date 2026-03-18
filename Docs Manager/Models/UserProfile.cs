using SQLite;

namespace Docs_Manager.Models;

[Table("UserProfile")]
public class UserProfile
{
    [PrimaryKey]
    public int Id { get; set; } = 1;

    // ===== ОСНОВНАЯ ИНФОРМАЦИЯ =====
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public int Age { get; set; }

    // ===== ПРОФЕССИОНАЛЬНАЯ ИНФОРМАЦИЯ =====
    public string? Position { get; set; }
    public string? Citizenship { get; set; }
    public string? Residence { get; set; }
    public string? ResidenceAirport { get; set; }
    public decimal DesiredWage { get; set; }

    // ===== ЛИЧНАЯ ИНФОРМАЦИЯ =====
    public int Height { get; set; } // см
    public int Weight { get; set; } // кг
    public int ShoeSize { get; set; }
    public int OverallSize { get; set; }
    public string? HairColor { get; set; }
    public string? EyeColor { get; set; }

    // ===== ОБРАЗОВАНИЕ =====
    public string? QualificationDegree { get; set; }
    public string? EducationInstitution { get; set; }
    public DateTime GraduationDate { get; set; }

    // ===== ФОТО =====
    public string? PhotoPath { get; set; } // Путь к фотографии

    // ===== ДОПОЛНИТЕЛЬНО =====
    public DateTime UpdatedDate { get; set; }

    // ПРИМЕЧАНИЕ: Документы хранятся в StoredFile (категория "documents")
    // Сертификаты - в Certificate (категория "certificates")
    // COC & Endorsement - в StoredFile (категория "coc")
}