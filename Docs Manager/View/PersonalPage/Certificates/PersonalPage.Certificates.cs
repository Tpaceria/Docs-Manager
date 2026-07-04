using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class PersonalPage
{
    private async Task LoadCertificatesPreview()
    {
        try
        {
            var certificates =
                (await GetDatabase()
                    .GetCertificatesAsync())
                .Where(x => x.Category == "CERTIFICATES")
                .ToList();

            var rows = new List<Grid>();

            foreach (var cert in certificates)
            {
                rows.Add(
                    CreateThreeColumnRow(
                        cert.Document,
                        string.IsNullOrWhiteSpace(cert.Country)
                            ? "—"
                            : cert.Country,
                        cert.IsLifetime
                            ? "No Expiry"
                            : cert.ExpiryDate.ToString("dd.MM.yyyy"),
                        Color.FromArgb("#00d4ff")));
            }

            BuildTable(
                CertificatesPreviewContainer,
                rows);
        }
        catch
        {
        }
    }
}