using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class PersonalPage
{
    private async Task LoadCocPreview()
    {
        try
        {
            var cocs =
                (await GetDatabase()
                    .GetCertificatesAsync())
                    .Where(x => x.Category == "COC")
                    .ToList();

            CocPreviewContainer.Clear();

            foreach (var coc in cocs)
            {
                var row =
                    new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition(GridLength.Star),
                            new ColumnDefinition(GridLength.Star),
                            new ColumnDefinition(GridLength.Auto)
                        },

                        Padding =
                            new Thickness(0, 2)
                    };

                row.Add(
                    new Label
                    {
                        Text =
                            coc.Document,

                        TextColor =
                            Colors.White
                    }, 0, 0);

                row.Add(
                    new Label
                    {
                        Text =
                            coc.Number,

                        TextColor =
                            Colors.White
                    }, 1, 0);

                row.Add(
                    new Label
                    {
                        Text =
                            coc.IsLifetime
                                ? "Lifetime"
                                : coc.ExpiryDate.Year.ToString(),

                        TextColor =
                            Color.FromArgb("#00d4ff")
                    }, 2, 0);

                CocPreviewContainer.Add(row);
            }
        }
        catch
        {
        }
    }
}