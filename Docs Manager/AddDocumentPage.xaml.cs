using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddDocumentPage : ContentView
{
    private readonly DatabaseService _database;

    private Document? _document;

    private string? _selectedFilePath;

    private readonly DocumentsPage _parentPage;

    private readonly MainPage _mainPage;

    public AddDocumentPage(
        DocumentsPage parentPage,
        MainPage mainPage)
    {
        InitializeComponent();

        _parentPage = parentPage;
        _mainPage = mainPage;

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        IssueDateControl.SelectedDate = DateTime.Today;
        ExpiryDateControl.SelectedDate = DateTime.Today.AddYears(5);
    }

    public AddDocumentPage(
        Document document,
        DocumentsPage parentPage,
        MainPage mainPage)
        : this(parentPage, mainPage)
    {
        _document = document;

        FillForm();
    }

    private void FillForm()
    {
        if (_document == null)
            return;

        DocumentTypePicker.SelectedItem = _document.Type;

        DocumentEntry.Text = _document.Title;

        CountryEntry.Text = _document.Country ?? "";

        NumberEntry.Text = _document.Number ?? "";

        IssueDateControl.SelectedDate = _document.IssueDate;

        LifetimeSwitch.IsToggled = _document.Lifetime;

        if (!_document.Lifetime)
            ExpiryDateControl.SelectedDate = _document.ExpiryDate;
        else
            ExpiryDateControl.SelectedDate = DateTime.Today;

        ExpiryStack.IsVisible = !_document.Lifetime;

        _selectedFilePath = _document.FilePath;

        if (!string.IsNullOrWhiteSpace(_selectedFilePath) &&
            File.Exists(_selectedFilePath))
        {
            FileInfoBorder.IsVisible = true;

            FileNameLabel.Text = Path.GetFileName(_selectedFilePath);

            FileSizeLabel.Text =
                $"Size: {FormatFileSize(new FileInfo(_selectedFilePath).Length)}";

            PickFileButton.Text = "✅ File Selected";

            PickFileButton.BackgroundColor =
                Color.FromArgb("#28A745");
        }
    }

    private void OnLifetimeToggled(object sender, ToggledEventArgs e)
    {
        ExpiryStack.IsVisible = !e.Value;
    }

    private async void OnPickFileClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(
                new PickOptions
                {
                    PickerTitle = "Select File"
                });

            if (result == null)
                return;

            _selectedFilePath = result.FullPath;

            var info = new FileInfo(_selectedFilePath);

            FileInfoBorder.IsVisible = true;

            FileNameLabel.Text = result.FileName;

            FileSizeLabel.Text =
                $"Size: {FormatFileSize(info.Length)}";

            PickFileButton.Text = "✅ File Selected";

            PickFileButton.BackgroundColor =
                Color.FromArgb("#28A745");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (DocumentTypePicker.SelectedIndex == -1)
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Error",
                    "Please select document type.",
                    "OK");

                return;
            }

            if (string.IsNullOrWhiteSpace(DocumentEntry.Text))
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Error",
                    "Please enter document title.",
                    "OK");

                return;
            }

            var document = new Document
            {
                Id = _document?.Id ?? 0,

                Type = DocumentTypePicker.SelectedItem?.ToString(),

                Title = DocumentEntry.Text?.Trim(),

                Country = CountryEntry.Text?.Trim(),

                Number = NumberEntry.Text?.Trim(),

                IssueDate = IssueDateControl.SelectedDate,

                ExpiryDate = LifetimeSwitch.IsToggled
                    ? DateTime.MaxValue
                    : ExpiryDateControl.SelectedDate,

                Lifetime = LifetimeSwitch.IsToggled,

                FilePath = _selectedFilePath
            };

            await _database.SaveDocumentAsync(document);

            // Регистрация файла в FileManager
            if (!string.IsNullOrWhiteSpace(_selectedFilePath))
            {
                try
                {
                    await _database.RegisterAttachedFileAsync(
                        _selectedFilePath,
                        "Документы",
                        document.Title);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: Could not register file in FileManager: {ex.Message}");
                }
            }

            _parentPage.RefreshList();

            _mainPage.SetPage(new DocumentsPage(_mainPage));
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        _mainPage.SetPage(new DocumentsPage(_mainPage));
    }

    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };

        double len = bytes;

        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }

    private void OnDocumentTypeChanged(object sender, EventArgs e)
    {
        if (_document != null)
            return;

        if (DocumentTypePicker.SelectedItem == null)
            return;

        switch (DocumentTypePicker.SelectedItem.ToString())
        {
            case "Passport":
                DocumentEntry.Text = "Travel Passport";
                break;

            case "Seaman Book":
                DocumentEntry.Text = "Seaman Book";
                break;

            case "Residence Permit":
                DocumentEntry.Text = "Residence Permit";
                break;

            case "National ID":
                DocumentEntry.Text = "National ID Card";
                break;

            case "Visa":
                DocumentEntry.Text = "Visa";
                break;
        }
    }
}
