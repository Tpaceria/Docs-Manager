using Docs_Manager.Data;
using Docs_Manager.Models;
using Microsoft.Maui.Controls;

namespace Docs_Manager.View;

public partial class AddVisaPage : ContentPage
{
    private readonly DatabaseService _database;


    public AddVisaPage()
    {
        InitializeComponent();

        _database =
            ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException();

        _ = LoadVisasAsync();
    }

    private async Task LoadVisasAsync()
    {
        var visas =
            await _database.GetVisasAsync();

        VisaContainer.Children.Clear();

        foreach (var visa in visas)
        {
            AddVisaBlock(visa);
        }

        if (VisaContainer.Children.Count == 0)
        {
            AddVisaBlock();
        }
    }

    private void AddVisaBlock(
        VisaModel? visa = null)
    {
        var deleteButton = new Button
        {
            Text = "🗑",
            WidthRequest = 34,
            HeightRequest = 34,
            CornerRadius = 17,
            BackgroundColor = Color.FromArgb("#ff4d6d"),
            TextColor = Colors.White,
            FontSize = 12
        };

        var headerGrid = new Grid
        {
            ColumnDefinitions =
        {
            new ColumnDefinition(GridLength.Star),
            new ColumnDefinition(GridLength.Auto)
        }
        };

        headerGrid.Add(new Label
        {
            Text = "Visa",
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold
        });

        headerGrid.Add(deleteButton, 1, 0);

        var typeEntry = new Entry
        {
            Placeholder = "US C1/D",
            Text = visa?.Type ?? "",
            TextColor = Colors.White,
            PlaceholderColor = Color.FromArgb("#7a8999"),
            BackgroundColor = Color.FromArgb("#1a2238")
        };

        var countryEntry = new Entry
        {
            Placeholder = "USA",
            Text = visa?.Country ?? "",
            TextColor = Colors.White,
            PlaceholderColor = Color.FromArgb("#7a8999"),
            BackgroundColor = Color.FromArgb("#1a2238")
        };

        var expiryPicker = new DatePicker
        {
            Date = visa?.ExpiryDate
                ?? DateTime.Today.AddYears(1),

            TextColor = Colors.White,
            BackgroundColor = Color.FromArgb("#1a2238")
        };

        var layout = new VerticalStackLayout
        {
            Spacing = 10,
            Children =
        {
            headerGrid,

            new Label
            {
                Text = "Type",
                TextColor = Colors.White
            },

            typeEntry,

            new Label
            {
                Text = "Country",
                TextColor = Colors.White
            },

            countryEntry,

            new Label
            {
                Text = "Expiry Date",
                TextColor = Colors.White
            },

            expiryPicker
        }
        };

        var frame = new Frame
        {
            BackgroundColor = Color.FromArgb("#163454"),
            BorderColor = Color.FromArgb("#2d4d73"),
            CornerRadius = 10,
            Padding = 10,
            HasShadow = false,
            Content = layout
        };

        deleteButton.Clicked += (s, e) =>
        {
            VisaContainer.Children.Remove(frame);
        };

        VisaContainer.Children.Add(frame);
    }

    private async void OnSaveClicked(
        object sender,
        EventArgs e)
    {
        try
        {
            // Удаляем старые визы

            var oldVisas =
                await _database.GetVisasAsync();

            foreach (var visa in oldVisas)
            {
                await _database.DeleteVisaAsync(visa);
            }

            // Сохраняем новые

            foreach (var child in VisaContainer.Children)
            {
                if (child is Frame frame
                    && frame.Content is VerticalStackLayout layout)
                {
                    var entries =
                        layout.Children
                            .OfType<Entry>()
                            .ToList();

                    var datePicker =
                        layout.Children
                            .OfType<DatePicker>()
                            .FirstOrDefault();

                    if (entries.Count < 2 ||
                        datePicker == null)
                        continue;

                    var type =
                        entries[0].Text ?? "";

                    var country =
                        entries[1].Text ?? "";

                    if (string.IsNullOrWhiteSpace(type))
                        continue;

                    await _database.SaveVisaAsync(
                        new VisaModel
                        {
                            Type = type,
                            Country = country,
                            ExpiryDate = datePicker.Date.Value
                        });
                }
            }

            await DisplayAlert(
                "Saved",
                "Visas saved successfully",
                "OK");

            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }    
    
    private void OnAddVisaClicked(
        object sender,
        EventArgs e)
    {
        AddVisaBlock();
    }
    private async void OnCloseClicked(
    object sender,
    EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
