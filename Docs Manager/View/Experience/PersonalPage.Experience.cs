using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class PersonalPage
{
    private async Task LoadExperiencePreview()
    {
        try
        {
            var experiences =
                await GetDatabase()
                    .GetExperiencesAsync();

            var rows = new List<Grid>();

            foreach (var experience in experiences)
            {
                rows.Add(
                    CreateFiveColumnRow(
                        experience.VesselName ?? "",
                        experience.Position,
                        experience.VesselType ?? "",
                        experience.DWT == 0
                            ? "—"
                            : experience.DWT.ToString("N0"),
                        $"{experience.SignOnDate:dd.MM.yyyy} - {experience.SignOffDate:dd.MM.yyyy}"));

            }

            BuildTable(
                ExperiencePreviewContainer,
                rows);
        }
        catch
        {
        }
    }
}