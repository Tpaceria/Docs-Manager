using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddCocPage : ContentPage
{
    readonly DatabaseService _database;
    Certificate _certificate;
    string _selectedFilePath;

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
            DocumentEntry.Text = _certificate.Document;
            CountryEntry.Text = _certificate.Country ?? "";
            NumberEntry.Text = _certificate.Number;
            IssueDatePicker.Date = _certificate.IssueDate;
            LifetimeSwitch.IsToggled = _certificate.IsLifetime;
            ExpiryDatePicker.Date = _certificate.ExpiryDate;
            _selectedFilePath = _certificate.FilePath;

            if (!string.IsNullOrEmpty(_selectedFilePath))
            {
                FileInfoBorder.IsVisible = true;
                FileNameLabel.Text = Path.GetFileName(_selectedFilePath);
                FileSizeLabel.Text = $"Size: {FormatFileSize(new FileInfo(_selectedFilePath).Length)}";
                PickFileButton.Text = "✅ File Selected";
                PickFileButton.BackgroundColor = Color.FromArgb("#28A745");
            }

            Title = "Edit COC & Endorsement";
            ExpiryStack.IsVisible = !_certificate.IsLifetime;
        }
        else
        {
            IssueDatePicker.Date = DateTime.Today;
            ExpiryDatePicker.Date = DateTime.Today.AddYears(5);
        }
    }

    void OnLifetimeToggled(object sender, ToggledEventArgs e)
    {
        ExpiryStack.IsVisible = !e.Value;
    }

    async void OnPickFileClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select File"
            });

            if (result != null)
            {
                _selectedFilePath = result.FullPath;
                var fileInfo = new FileInfo(_selectedFilePath);
                long fileSize = fileInfo.Length;

                FileInfoBorder.IsVisible = true;
                FileNameLabel.Text = result.FileName;
                FileSizeLabel.Text = $"Size: {FormatFileSize(fileSize)}";
                PickFileButton.Text = "✅ File Selected";
                PickFileButton.BackgroundColor = Color.FromArgb("#28A745");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(DocumentEntry.Text))
            {
                await DisplayAlert("Error", "Enter document name", "OK");
                return;
            }

            var certificate = new Certificate
            {
                Id = _certificate?.Id ?? 0,
                Document = DocumentEntry.Text,
                Country = CountryEntry.Text ?? "",
                Number = NumberEntry.Text ?? "",
                IssueDate = IssueDatePicker.Date ?? DateTime.Today,
                ExpiryDate = LifetimeSwitch.IsToggled ? DateTime.MaxValue : (ExpiryDatePicker.Date ?? DateTime.Today),
                IsLifetime = LifetimeSwitch.IsToggled,
                FilePath = _selectedFilePath,
                Category = "COC & ENDORSEMENT"
            };

            await _database.SaveCertificateAsync(certificate);
            await DisplayAlert("Success", "COC & Endorsement saved!", "OK");
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
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