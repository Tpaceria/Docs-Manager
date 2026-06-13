using SQLite;

namespace Docs_Manager.Models;

public class BiometricModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int Height { get; set; }

    public int Weight { get; set; }

    public int ShoeSize { get; set; }

    public int OverallSize { get; set; }

    public string HairColor { get; set; } = "";

    public string EyeColor { get; set; } = "";
}