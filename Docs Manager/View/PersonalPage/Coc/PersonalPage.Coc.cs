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

            var rows = new List<Grid>();

            foreach (var coc in cocs)
            {
                rows.Add(
                    CreateThreeColumnRow(
                        coc.Document,
                        coc.Number,
                        coc.IsLifetime
                            ? "No Expiry"
                            : coc.ExpiryDate.ToString("dd.MM.yyyy"),
                        Color.FromArgb("#00d4ff")));
            }

            BuildTable(
                CocPreviewContainer,
                rows);
        }
        catch
        {
        }
    }
}