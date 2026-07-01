using Docs_Manager.Data;
using Docs_Manager.Models;
using Docs_Manager.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace Docs_Manager.View;

public partial class AddEducationPage : ContentPage
{
    private readonly DatabaseService _database;
    private class EducationBlock
    {
        public Border Border { get; set; } = null!;

        public Entry QualificationEntry { get; set; } = null!;

        public Entry InstitutionEntry { get; set; } = null!;

        public Entry CountryEntry { get; set; } = null!;

        public Picker InstitutionTypePicker { get; set; } = null!;

        public DateSelector GraduationSelector { get; set; } = null!;

        public Button FileButton { get; set; } = null!;

        public string? FilePath { get; set; }

        public int Id { get; set; }
    }
    private readonly List<EducationBlock> _blocks = new();

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

    private void AddEducationBlock(EducationInfo? education = null)
    {
        var block = new EducationBlock();

        block.Id = education?.Id ?? 0;

        block.FilePath = education?.FilePath;

        block.QualificationEntry = new Entry
        {
            Placeholder = "Qualification / Degree",
            Text = education?.Qualification,
            BackgroundColor = Color.FromArgb("#1a2238"),
            TextColor = Colors.White,
            PlaceholderColor = Color.FromArgb("#7a8999")
        };

        block.InstitutionEntry = new Entry
        {
            Placeholder = "Institution",
            Text = education?.Institution,
            BackgroundColor = Color.FromArgb("#1a2238"),
            TextColor = Colors.White,
            PlaceholderColor = Color.FromArgb("#7a8999")
        };

        block.CountryEntry = new Entry
        {
            Placeholder = "Country",
            Text = education?.Country,
            BackgroundColor = Color.FromArgb("#1a2238"),
            TextColor = Colors.White,
            PlaceholderColor = Color.FromArgb("#7a8999")
        };

        block.InstitutionTypePicker = new Picker
        {
            BackgroundColor = Color.FromArgb("#1a2238"),
            TextColor = Colors.White
        };

        block.InstitutionTypePicker.Items.Add("Maritime Academy");
        block.InstitutionTypePicker.Items.Add("University");
        block.InstitutionTypePicker.Items.Add("College");
        block.InstitutionTypePicker.Items.Add("Training Center");
        block.InstitutionTypePicker.Items.Add("School");
        block.InstitutionTypePicker.Items.Add("Other");

        if (!string.IsNullOrWhiteSpace(education?.InstitutionType))
        {
            block.InstitutionTypePicker.SelectedItem =
                education.InstitutionType;
        }


        block.GraduationSelector = new DateSelector
        {
            SelectedDate =
                education?.GraduationDate ??
                DateTime.Today
        };

        block.FileButton = new Button
        {
            Text =
                string.IsNullOrWhiteSpace(block.FilePath)
                    ? "📎 Choose Diploma"
                    : "📎 Diploma Selected",

            BackgroundColor =
                Color.FromArgb("#00709f"),

            TextColor = Colors.White,

            CornerRadius = 8
        };

        block.FileButton.Clicked += async (s, e) =>
        {
            var result =
                await FilePicker.Default.PickAsync();

            if (result == null)
                return;

            block.FilePath =
                result.FullPath;

            block.FileButton.Text =
                "📎 Diploma Selected";
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

                TextColor = Colors.White
            };
        deleteButton.Clicked += async (s, e) =>
        {
            bool confirm = await DisplayAlert(
                "Delete",
                "Delete education record?",
                "Yes",
                "Cancel");

            if (!confirm)
                return;

            if (block.Id != 0)
            {
                await _database.DeleteEducationAsync(
                    new EducationInfo { Id = block.Id });
            }

            _blocks.Remove(block);

            EducationContainer.Remove(block.Border);

            if (EducationContainer.Count == 0)
            {
                AddEducationBlock();
            }
        };

        var header =
            new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };

        header.Add(
            new Label
            {
                Text = $"Education #{_blocks.Count + 1}",

                FontSize = 16,

                FontAttributes = FontAttributes.Bold,

                TextColor = Colors.White
            });

        header.Add(deleteButton);

        Grid.SetColumn(deleteButton, 1);

        var layout =
            new VerticalStackLayout
            {
                Spacing = 12
            };

        layout.Add(header);

        layout.Add(SectionLabel("Country"));
        layout.Add(block.CountryEntry);

        layout.Add(SectionLabel("Institution Type"));
        layout.Add(block.InstitutionTypePicker);

        layout.Add(SectionLabel("Institution"));
        layout.Add(block.InstitutionEntry);

        layout.Add(SectionLabel("Degree / Qualification"));
        layout.Add(block.QualificationEntry);

        layout.Add(SectionLabel("Graduation"));
        layout.Add(block.GraduationSelector);

        layout.Add(SectionLabel("Diploma"));
        layout.Add(block.FileButton);
        block.Border =
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

                Padding = 12,

                Content = layout
            };

        _blocks.Add(block);

        EducationContainer.Add(block.Border);
    }
    private Label SectionLabel(string text)
    {
        return new Label
        {
            Text = text,
            FontSize = 13,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#9FB4CC")
        };
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
            foreach (var block in _blocks)
            {
                if (string.IsNullOrWhiteSpace(
                    block.QualificationEntry.Text))
                {
                    continue;
                }

                var education =
                    new EducationInfo
                    {
                        Id = block.Id,

                        Country =
    block.CountryEntry.Text ?? "",

                        InstitutionType =
    block.InstitutionTypePicker.SelectedItem?.ToString() ?? "",

                        Institution =
    block.InstitutionEntry.Text ?? "",

                        Qualification =
    block.QualificationEntry.Text ?? "",

                        GraduationDate =
                            block.GraduationSelector.SelectedDate,

                        FilePath =
                            block.FilePath
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