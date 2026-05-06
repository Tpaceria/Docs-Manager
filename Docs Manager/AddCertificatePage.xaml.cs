using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddCertificatePage : ContentPage
{
    private string _selectedFilePath;
    private readonly CertificatePage _parentPage;
    private readonly MainPage _mainPage;
    private Certificate _certificate;

    public AddCertificatePage(CertificatePage parentPage, MainPage mainPage)
    {
        InitializeComponent();

        _parentPage = parentPage;
        _mainPage = mainPage;

        Title = "Add Certificate";
    }

    public AddCertificatePage(
        Certificate certificate,
        CertificatePage parentPage,
        MainPage mainPage) : this(parentPage, mainPage)
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

        IssueDatePicker.Date = Convert.ToDateTime(_certificate.IssueDate);
        ExpiryDatePicker.Date = Convert.ToDateTime(_certificate.ExpiryDate);

        LifetimeSwitch.IsToggled = _certificate.IsLifetime;
        _selectedFilePath = _certificate.FilePath;

        if (!string.IsNullOrEmpty(_selectedFilePath))
        {
            FileNameLabel.Text = Path.GetFileName(_selectedFilePath);
        }

        Title = "Edit Certificate";
        ExpiryStack.IsVisible = !_certificate.IsLifetime;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_certificate == null)
        {
            IssueDatePicker.Date = DateTime.Today;
            ExpiryDatePicker.Date = DateTime.Today.AddYears(5);
        }
    }

    private async void OnPickFileClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync();

        if (result != null)
        {
            _selectedFilePath = result.FullPath;
            FileNameLabel.Text = Path.GetFileName(_selectedFilePath);
        }
    }

    private void OnLifetimeToggled(object sender, ToggledEventArgs e)
    {
        ExpiryStack.IsVisible = !e.Value;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DocumentEntry.Text))
        {
            await DisplayAlert("Error", "Document name is required", "OK");
            return;
        }

        if (_certificate == null)
        {
            var cert = new Certificate
            {
                Document = DocumentEntry.Text,
                Country = CountryEntry.Text,
                Number = NumberEntry.Text,
                IssueDate = Convert.ToDateTime(IssueDatePicker.Date),
                ExpiryDate = LifetimeSwitch.IsToggled
                    ? DateTime.MaxValue
                    : Convert.ToDateTime(ExpiryDatePicker.Date),
                IsLifetime = LifetimeSwitch.IsToggled,
                FilePath = _selectedFilePath
            };

            _parentPage.AddCertificate(cert);
        }
        else
        {
            _certificate.Document = DocumentEntry.Text;
            _certificate.Country = CountryEntry.Text;
            _certificate.Number = NumberEntry.Text;

            _certificate.IssueDate =
                Convert.ToDateTime(IssueDatePicker.Date);

            _certificate.ExpiryDate = LifetimeSwitch.IsToggled
                ? DateTime.MaxValue
                : Convert.ToDateTime(ExpiryDatePicker.Date);

            _certificate.IsLifetime = LifetimeSwitch.IsToggled;
            _certificate.FilePath = _selectedFilePath;

            _parentPage.AddCertificate(_certificate);
        }

        // ✅ возврат на список
        _mainPage.SetPage(new CertificatePage(_mainPage));
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        // ✅ возврат без сохранения
        _mainPage.SetPage(new CertificatePage(_mainPage));
    }
}