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

            ExperiencePreviewContainer.Clear();

            foreach (var experience in experiences)
            {
                var row =
                    new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition(GridLength.Star),
                            new ColumnDefinition(GridLength.Star),
                            new ColumnDefinition(GridLength.Star),
                            new ColumnDefinition(GridLength.Star)
                        },

                        Padding =
                            new Thickness(0, 2)
                    };

                row.Add(
                    new Label
                    {
                        Text =
                            experience.VesselName,

                        TextColor =
                            Colors.White
                    }, 0, 0);

                row.Add(
                    new Label
                    {
                        Text =
                            experience.Position,

                        TextColor =
                            Colors.White
                    }, 1, 0);

                row.Add(
                    new Label
                    {
                        Text =
                            experience.CrewingAgency,

                        TextColor =
                            Colors.White
                    }, 2, 0);

                row.Add(
                    new Label
                    {
                        Text =
                            $"{experience.SignOnDate:yyyy}-{experience.SignOffDate:yyyy}",

                        TextColor =
                            Color.FromArgb("#00d4ff")
                    }, 3, 0);

                ExperiencePreviewContainer.Add(row);
            }
        }
        catch
        {
        }
    }
}