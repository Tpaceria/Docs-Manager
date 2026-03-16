using SQLite;
using System;

namespace Docs_Manager.Models
{
    [Table("Experiences")]
    public class Experience
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(200)]
        public string? VesselName { get; set; }

        [MaxLength(100)]
        public string? Position { get; set; }

        [MaxLength(100)]
        public string? VesselType { get; set; }

        public int DWT { get; set; }

        [MaxLength(100)]
        public string? Flag { get; set; }

        public int YearOfBuilt { get; set; }

        public DateTime SignOnDate { get; set; }

        public DateTime SignOffDate { get; set; }

        [MaxLength(200)]
        public string? Shipowner { get; set; }

        [MaxLength(200)]
        public string? CrewingAgency { get; set; }

        public int MainEngineKW { get; set; }

        [MaxLength(100)]
        public string? METype { get; set; }

        [MaxLength(100)]
        public string? IMO { get; set; }

        public bool IsVessel { get; set; } = true;

        [Ignore]
        public string DateRange => $"{VesselName} from {SignOnDate:dd.MM.yyyy} to {SignOffDate:dd.MM.yyyy}";
    }
}