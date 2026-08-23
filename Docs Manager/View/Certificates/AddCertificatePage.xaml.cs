using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddCertificatePage : ContentView
{
    private string _selectedFilePath;

    private readonly CertificatePage _parentPage;

    private readonly MainPage _mainPage;

    private Certificate _certificate;

    private readonly DatabaseService _database;

    public AddCertificatePage(
        CertificatePage parentPage,
        MainPage mainPage)
    {
        InitializeComponent();

        _parentPage = parentPage;
        _mainPage = mainPage;

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        if (_certificate == null)
        {
            IssueDateControl.SelectedDate =
                DateTime.Today;

            ExpiryDateControl.SelectedDate =
                DateTime.Today.AddYears(1);
        }
    }

    public AddCertificatePage(
        Certificate certificate,
        CertificatePage parentPage,
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

        DocumentEntry.Text =
            _certificate.Document;

        CountryEntry.Text =
            _certificate.Country ?? "";

        NumberEntry.Text =
            _certificate.Number;

        IssueDateControl.SelectedDate =
            _certificate.IssueDate;

        ExpiryDateControl.SelectedDate =
            _certificate.ExpiryDate;

        LifetimeSwitch.IsToggled =
            _certificate.IsLifetime;

        _selectedFilePath =
            _certificate.FilePath;

        if (!string.IsNullOrEmpty(_selectedFilePath))
        {
            FileNameLabel.Text =
                Path.GetFileName(_selectedFilePath);
        }

        ExpiryStack.IsVisible =
            !_certificate.IsLifetime;
    }

    private async void OnPickFileClicked(
        object sender,
        EventArgs e)
    {
        var result =
            await FilePicker.Default.PickAsync();

        if (result != null)
        {
            _selectedFilePath =
                result.FullPath;

            FileNameLabel.Text =
                Path.GetFileName(_selectedFilePath);
        }
    }

    private void OnLifetimeToggled(
        object sender,
        ToggledEventArgs e)
    {
        ExpiryStack.IsVisible =
            !e.Value;
    }

    private async void OnSaveClicked(
        object sender,
        EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
            DocumentEntry.Text))
        {
            Application.Current!.MainPage!.DisplayAlert(
                "Error",
                "Document name is required",
                "OK");

            return;
        }

        if (_certificate == null)
        {
            var cert =
                new Certificate
                {
                    Document =
                        DocumentEntry.Text,

                    Country =
                        CountryEntry.Text,

                    Number =
                        NumberEntry.Text,

                    IssueDate =
                        IssueDateControl.SelectedDate,

                    ExpiryDate =
                        LifetimeSwitch.IsToggled
                            ? DateTime.MaxValue
                            : ExpiryDateControl.SelectedDate,

                    IsLifetime =
                        LifetimeSwitch.IsToggled,

                    FilePath =
                        _selectedFilePath,

                    Category = "CERTIFICATES"
                };

            _parentPage.AddCertificate(cert);

            // Регистрация файла в FileManager
            if (!string.IsNullOrWhiteSpace(_selectedFilePath))
            {
                try
                {
                    await _database.RegisterAttachedFileAsync(
                        _selectedFilePath,
                        "CERTIFICATES",
                        cert.Document);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: Could not register file in FileManager: {ex.Message}");
                }
            }
        }
        else
        {
            _certificate.Document =
                DocumentEntry.Text;

            _certificate.Country =
                CountryEntry.Text;

            _certificate.Number =
                NumberEntry.Text;

            _certificate.IssueDate =
                IssueDateControl.SelectedDate;

            _certificate.ExpiryDate =
                LifetimeSwitch.IsToggled
                    ? DateTime.MaxValue
                    : ExpiryDateControl.SelectedDate;

            _certificate.IsLifetime =
                LifetimeSwitch.IsToggled;

            _certificate.FilePath =
                _selectedFilePath;

            _parentPage.AddCertificate(
                _certificate);

            // Регистрация файла в FileManager если он прикреплён
            if (!string.IsNullOrWhiteSpace(_selectedFilePath))
            {
                try
                {
                    await _database.RegisterAttachedFileAsync(
                        _selectedFilePath,
                        "CERTIFICATES",
                        _certificate.Document);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: Could not register file in FileManager: {ex.Message}");
                }
            }
        }

        _mainPage.SetPage(
            new CertificatePage(
                _mainPage));
    }

    private void OnCancelClicked(
        object sender,
        EventArgs e)
    {
        _mainPage.SetPage(
            new CertificatePage(
                _mainPage));
    }
}