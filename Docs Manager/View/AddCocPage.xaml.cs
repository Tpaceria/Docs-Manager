using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddCocPage : ContentPage
{
    private readonly DatabaseService _database;
    private Certificate? _certificate;
    private string? _selectedFilePath;

    public AddCocPage()
    {
        InitializeComponent();
        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");
    }

    public AddCocPage(Certificate certificate) : this()
    {
        _certificate = certificate;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_certificate != null)
        {
            CocNameEntry.Text = _certificate.Document;
            DescriptionEditor.Text = _certificate.Description ?? "";
            ExpirationDatePicker.Date = _certificate.ExpiryDate;
            _selectedFilePath = _certificate.FilePath;

            if (!string.IsNullOrEmpty(_selectedFilePath))
            {
                FileInfoStack.IsVisible = true;
                FileNameLabel.Text = Path.GetFileName(_selectedFilePath);
            }

            Title = "Edit COC";
        }
        else
        {
            ExpirationDatePicker.Date = DateTime.Today.AddYears(5);
        }
    }

    private async void OnPickFileClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select a file"
            });

            if (result != null)
            {
                _selectedFilePath = result.FullPath;
                var fileInfo = new FileInfo(_selectedFilePath);
                long fileSize = fileInfo.Length;

                FileInfoStack.IsVisible = true;
                FileNameLabel.Text = result.FileName;
                FileSizeLabel.Text = $"Size: {FormatFileSize(fileSize)}";
                PickFileButton.Text = "✅ File Selected";
                PickFileButton.BackgroundColor = Color.FromArgb("#28A745");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to pick file: {ex.Message}", "OK");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(CocNameEntry.Text))
        {
            await DisplayAlert("Error", "Please enter COC name", "OK");
            return;
        }

        try
        {
            var certificate = new Certificate
            {
                Id = _certificate?.Id ?? 0,
                Document = CocNameEntry.Text,
                Description = DescriptionEditor.Text ?? "",
                ExpiryDate = ExpirationDatePicker.Date ?? DateTime.Today,
                FilePath = _selectedFilePath,
                Category = "COC & ENDORSEMENT",
                IssueDate = DateTime.Today,
                Number = "",
                IsLifetime = false,
                Country = ""
            };

            await _database.SaveCertificateAsync(certificate);
            await DisplayAlert("Success", "COC saved!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}