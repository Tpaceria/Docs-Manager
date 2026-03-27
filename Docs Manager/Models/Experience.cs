using SQLite;

namespace Docs_Manager.Models;

[Table("experiences")]
public class Experience
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Column("vessel_name")]
    public string? VesselName { get; set; }

    [Column("dwt")]
    public int DWT { get; set; }

    [Column("position")]
    public string Position { get; set; } = string.Empty;

    [Column("vessel_type")]
    public string? VesselType { get; set; }

    [Column("flag")]
    public string? Flag { get; set; }

    [Column("year_of_built")]
    public int YearOfBuilt { get; set; }

    [Column("sign_on_date")]
    public DateTime SignOnDate { get; set; }

    [Column("sign_off_date")]
    public DateTime SignOffDate { get; set; }

    [Column("main_engine_kw")]
    public int MainEngineKW { get; set; }

    [Column("me_type")]
    public string? METype { get; set; }

    [Column("shipowner")]
    public string? Shipowner { get; set; }

    [Column("crewing_agency")]
    public string? CrewingAgency { get; set; }

    [Column("imo")]
    public string? IMO { get; set; }

    // Вычисляемые свойства для отображения
    public string Duration
    {
        get
        {
            var span = SignOffDate - SignOnDate;
            int months = span.Days / 30;
            return months > 0 ? $"{months} мес." : "< 1 мес.";
        }
    }
}