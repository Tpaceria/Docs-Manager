using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddCocPage : ContentView
{
    readonly DatabaseService _database;

    Certificate _certificate;

    string _selectedFilePath;

    private readonly CocEndorsementPage _parentPage;

    private readonly MainPage _mainPage;

    public AddCocPage(
        CocEndorsementPage parentPage,
        MainPage mainPage)
    {
        InitializeComponent();

        _parentPage = parentPage;
        _mainPage = mainPage;

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        if (_certificate == null)
        {
            IssueDateControl.SelectedDate = DateTime.Today;
            ExpiryDateControl.SelectedDate = DateTime.Today.AddYears(5);
        }
    }

    public AddCocPage(
        Certificate certificate,
        CocEndorsementPage parentPage,
        MainPage mainPage)
        : this(parentPage, mainPage)
    {
        _certificate = certificate;

        FillForm();
    }

    private void FillForm()
    {
        if (_certificate == null)
            return;

        DocumentEntry.Text = _certificate.Document;

        CountryEntry.Text = _certificate.Country ?? "";

        NumberEntry.Text = _certificate.Number;

        IssueDateControl.SelectedDate =
            Convert.ToDateTime(_certificate.IssueDate);

        ExpiryDateControl.SelectedDate =
            Convert.ToDateTime(_certificate.ExpiryDate);

        LifetimeSwitch.IsToggled =
            _certificate.IsLifetime;

        _selectedFilePath =
            _certificate.FilePath;

        if (!string.IsNullOrEmpty(_selectedFilePath))
        {
            FileInfoBorder.IsVisible = true;

            FileNameLabel.Text =
                Path.GetFileName(_selectedFilePath);

            FileSizeLabel.Text =
                $"Size: {FormatFileSize(new FileInfo(_selectedFilePath).Length)}";

            PickFileButton.Text = "✅ File Selected";

            PickFileButton.BackgroundColor =
                Color.FromArgb("#28A745");
        }

        ExpiryStack.IsVisible =
            !_certificate.IsLifetime;
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

                FileSizeLabel.Text =
                    $"Size: {FormatFileSize(fileSize)}";

                PickFileButton.Text = "✅ File Selected";

                PickFileButton.BackgroundColor =
                    Color.FromArgb("#28A745");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(DocumentEntry.Text))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Enter document name",
                    "OK");

                return;
            }

            var certificate = new Certificate
            {
                Id = _certificate?.Id ?? 0,

                Document = DocumentEntry.Text,

                Country = CountryEntry.Text ?? "",

                Number = NumberEntry.Text ?? "",

                IssueDate = IssueDateControl.SelectedDate,
                ExpiryDate = LifetimeSwitch.IsToggled
    ? DateTime.MaxValue
    : Convert.ToDateTime(ExpiryDateControl.SelectedDate),
                IsLifetime =
                    LifetimeSwitch.IsToggled,

                FilePath = _selectedFilePath,

                Category = "COC"
            };

            await _database.SaveCertificateAsync(certificate);

            if (_certificate == null)
            {
                _parentPage.AddCertificate(certificate);
            }
            else
            {
                _parentPage.RefreshList();
            }

            _mainPage.SetPage(new CocEndorsementPage(_mainPage));
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    void OnCancelClicked(object sender, EventArgs e)
    {
        _mainPage.SetPage(new CocEndorsementPage(_mainPage));
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