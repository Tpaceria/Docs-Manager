using System;
using System.Collections.Generic;
using System.Text;

namespace Docs_Manager.View;

public partial class PersonalPage
{
    private async void OnEditVisaClicked(
        object sender,
        EventArgs e)
    {
        var page =
            new AddVisaPage();

        page.Disappearing += async (s, e2) =>
        {
            await LoadVisaPreview();
        };

        await Navigation.PushModalAsync(page);
    }

    private async Task LoadVisaPreview()
    {
        try
        {
            var visas =
                await GetDatabase()
                    .GetVisasAsync();

            VisaPreviewContainer.Clear();

            foreach (var visa in visas)
            {
                var grid =
                    new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition
                            {
                                Width = GridLength.Star
                            },
                            new ColumnDefinition
                            {
                                Width = GridLength.Star
                            },
                            new ColumnDefinition
                            {
                                Width = GridLength.Auto
                            }
                        }
                    };

                grid.Add(
                    new Label
                    {
                        Text = visa.Type,
                        TextColor = Colors.White
                    }, 0, 0);

                grid.Add(
                    new Label
                    {
                        Text = visa.Country,
                        TextColor = Colors.White
                    }, 1, 0);

                grid.Add(
                    new Label
                    {
                        Text =
                            visa.ExpiryDate
                                .Year
                                .ToString(),

                        TextColor =
                            Color.FromArgb("#19b5ea")
                    }, 2, 0);

                VisaPreviewContainer.Add(grid);
            }
        }
        catch
        {
        }
    }
}
