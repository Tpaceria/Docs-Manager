using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class PersonalPage
{

    private async Task LoadDocumentsPreview()
    {
        try
        {
            var documents =
                await GetDatabase().GetDocumentsAsync();

            var rows = new List<Grid>();

            foreach (var document in documents)
            {
                rows.Add(
                    CreateThreeColumnRow(
                        document.Title ?? "",
                        document.Country ?? "",
                        document.Lifetime
                            ? "No Expiry"
                            : document.ExpiryDate.ToString("dd.MM.yyyy"),
                        Color.FromArgb("#19b5ea")));
            }

            DocumentsHeaderContainer.Clear();

            DocumentsHeaderContainer.Add(
                CreateThreeColumnHeader(
                    "Title",
                    "Country",
                    "Expiry"));

            BuildTable(DocumentsPreviewContainer, rows);
        }
        catch
        {
        }
    }
}