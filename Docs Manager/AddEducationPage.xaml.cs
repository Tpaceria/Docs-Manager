using Docs_Manager.Data;
using Docs_Manager.Models;
using Microsoft.Maui.Controls.Shapes;

namespace Docs_Manager.View;

public partial class AddEducationPage : ContentPage
{
    private readonly DatabaseService _database;

    public AddEducationPage()
    {
        InitializeComponent();

        _database =
            ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException(
                "DatabaseService not found");

        _ = LoadEducationAsync();
    }

    // =====================================
    // LOAD
    // =====================================

    private async Task LoadEducationAsync()
    {
        try
        {
            var educationList =
                await _database.GetEducationAsync();

            EducationContainer.Clear();

            foreach (var education in educationList)
            {
                AddEducationBlock(education);
            }

            if (educationList.Count == 0)
            {
                AddEducationBlock();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    // =====================================
    // ADD BLOCK
    // =====================================

    private void OnAddEducationClicked(
        object sender,
        EventArgs e)
    {
        AddEducationBlock();
    }

    private void AddEducationBlock(
        EducationInfo? education = null)
    {
        var qualificationEntry =
            new Entry
            {
                Placeholder = "Qualification / Degree",

                BackgroundColor =
                    Color.FromArgb("#1a2238"),

                TextColor =
                    Colors.White,

                PlaceholderColor =
                    Color.FromArgb("#7a8999"),

                Text =
                    education?.Qualification
            };

        var institutionEntry =
            new Entry
            {
                Placeholder = "University / Academy",

                BackgroundColor =
                    Color.FromArgb("#1a2238"),

                TextColor =
                    Colors.White,

                PlaceholderColor =
                    Color.FromArgb("#7a8999"),

                Text =
                    education?.Institution
            };

        var graduationPicker =
            new DatePicker
            {
                BackgroundColor =
                    Color.FromArgb("#1a2238"),

                TextColor =
                    Colors.White,

                Date =
                    education != null
                        ? education.GraduationDate
                        : DateTime.Today
            };

        var deleteButton =
            new Button
            {
                Text = "🗑",

                WidthRequest = 38,
                HeightRequest = 38,

                CornerRadius = 19,

                BackgroundColor =
                    Color.FromArgb("#ff4d6d"),

                TextColor =
                    Colors.White
            };

        var border =
            new Border
            {
                BackgroundColor =
                    Color.FromArgb("#163555"),

                Stroke =
                    Color.FromArgb("#224b75"),

                StrokeShape =
                    new RoundRectangle
                    {
                        CornerRadius = 12
                    },

                Padding = 12
            };

        var layout =
            new VerticalStackLayout
            {
                Spacing = 12
            };

        // =====================================
        // IDS
        // =====================================

        string educationId =
            education?.Id.ToString() ?? "0";

        qualificationEntry.ClassId =
            educationId;

        institutionEntry.ClassId =
            educationId;

        graduationPicker.ClassId =
            educationId;

        // =====================================
        // DELETE
        // =====================================

        deleteButton.Clicked += async (s, e) =>
        {
            bool confirm =
                await DisplayAlert(
                    "Delete",
                    "Delete education record?",
                    "Yes",
                    "Cancel");

            if (!confirm)
                return;

            if (education != null)
            {
                await _database.DeleteEducationAsync(
                    education);
            }

            EducationContainer.Remove(border);

            if (EducationContainer.Count == 0)
            {
                AddEducationBlock();
            }
        };

        // =====================================
        // HEADER
        // =====================================

        var header =
            new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(
                        GridLength.Star),

                    new ColumnDefinition(
                        GridLength.Auto)
                }
            };

        var titleLabel =
            new Label
            {
                Text = "Education",

                FontSize = 16,

                FontAttributes =
                    FontAttributes.Bold,

                TextColor =
                    Colors.White
            };

        header.Add(titleLabel);

        header.Add(deleteButton);

        Grid.SetColumn(deleteButton, 1);

        // =====================================
        // CONTENT
        // =====================================

        layout.Add(header);

        layout.Add(qualificationEntry);

        layout.Add(institutionEntry);

        layout.Add(graduationPicker);

        border.Content = layout;

        EducationContainer.Add(border);
    }

    // =====================================
    // SAVE
    // =====================================

    private async void OnSaveClicked(
        object sender,
        EventArgs e)
    {
        try
        {
            foreach (var item in EducationContainer)
            {
                if (item is not Border border)
                    continue;

                if (border.Content is not VerticalStackLayout layout)
                    continue;

                var entries =
                    layout.Children
                        .OfType<Entry>()
                        .ToList();

                var qualificationEntry =
                    entries.ElementAtOrDefault(0);

                var institutionEntry =
                    entries.ElementAtOrDefault(1);

                var graduationPicker =
                    layout.Children
                        .OfType<DatePicker>()
                        .FirstOrDefault();

                if (qualificationEntry == null ||
                    institutionEntry == null ||
                    graduationPicker == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(
                    qualificationEntry.Text))
                {
                    continue;
                }

                int.TryParse(
                    qualificationEntry.ClassId,
                    out int educationId);

                var education =
                    new EducationInfo
                    {
                        Id = educationId,

                        Qualification =
                            qualificationEntry.Text ?? "",

                        Institution =
                            institutionEntry.Text ?? "",

                        GraduationDate =
    graduationPicker?.Date ?? DateTime.Today
                    };

                await _database.SaveEducationAsync(
                    education);
            }

            await DisplayAlert(
                "Saved",
                "Education saved successfully",
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

    // =====================================
    // CLOSE
    // =====================================

    private async void OnCloseClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}