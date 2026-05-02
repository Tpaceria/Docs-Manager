using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddCertificatePage : ContentPage
{
    readonly DatabaseService _database;
    Certificate _certificate;
    string _selectedFilePath;
    CertificatePage _parentPage;

    public AddCertificatePage(CertificatePage parentPage)
    {
        InitializeComponent();
        _parentPage = parentPage;

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");
    }

    public AddCertificatePage(Certificate certificate, CertificatePage parentPage) : this(parentPage)
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

            Title = "Edit Certificate";
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
                ExpiryDate = LifetimeSwitch.IsToggled
    ? DateTime.MaxValue
    : (ExpiryDatePicker.Date ?? DateTime.Today),
                IsLifetime = LifetimeSwitch.IsToggled,
                FilePath = _selectedFilePath,
                Category = "CERTIFICATES"
            };

            await _database.SaveCertificateAsync(certificate);

            await DisplayAlert("Success", "Certificate saved!", "OK");

            // 🔥 обновляем список
            if (_parentPage != null)
                await _parentPage.LoadCertificatesPublic();

            // 🔥 возвращаемся назад
            if (Application.Current.MainPage is MainPage main)
                main.SetPage(_parentPage);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        if (Application.Current.MainPage is MainPage main)
            main.SetPage(_parentPage);
    }

    async void OnPickFileClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync();

            if (result != null)
            {
                _selectedFilePath = result.FullPath;
                await DisplayAlert("File selected", result.FileName, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}