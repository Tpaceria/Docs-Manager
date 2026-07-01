using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class PersonalPage
{
    private async Task LoadCertificatesPreview()
    {
        try
        {
            var certificates =
                await GetDatabase()
                    .GetCertificatesAsync();

            CertificatesPreviewContainer.Clear();

            foreach (var certificate in certificates)
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
                            certificate.Document,

                        TextColor =
                            Colors.White
                    }, 0, 0);

                row.Add(
                    new Label
                    {
                        Text =
                            certificate.Country,

                        TextColor =
                            Colors.White
                    }, 1, 0);

                row.Add(
                    new Label
                    {
                        Text =
                            certificate.IsLifetime
                                ? "Lifetime"
                                : certificate.ExpiryDate.Year.ToString(),

                        TextColor =
                            Color.FromArgb("#00d4ff")
                    }, 2, 0);

                CertificatesPreviewContainer.Add(row);
            }
        }
        catch
        {
        }
    }
}