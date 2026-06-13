using SQLite;

namespace Docs_Manager.Models;

public class SkillsModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string EnglishLevel { get; set; } = "";

    public string MarlinsScore { get; set; } = "";

    public string CesScore { get; set; } = "";

    public string AdditionalLanguages { get; set; } = "";
}