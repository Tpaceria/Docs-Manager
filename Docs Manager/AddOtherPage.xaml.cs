using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddOtherPage : ContentView
{
    private readonly DatabaseService _database;

    private readonly OtherPage _parentPage;

    private readonly MainPage _mainPage;

    private Certificate _certificate;

    private string _selectedFilePath;

    public AddOtherPage(
        OtherPage parentPage,
        MainPage mainPage)
    {
        InitializeComponent();

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        _parentPage = parentPage;

        _mainPage = mainPage;

        if (_certificate == null)
        {
            IssueDatePicker.Date = DateTime.Today;

            ExpiryDatePicker.Date =
                DateTime.Today.AddYears(5);
        }
    }

    public AddOtherPage(
        Certificate certificate,
        OtherPage parentPage,
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

        IssueDatePicker.Date =
            Convert.ToDateTime(_certificate.IssueDate);

        ExpiryDatePicker.Date =
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
        }

        ExpiryStack.IsVisible =
            !_certificate.IsLifetime;
    }

    private void OnLifetimeToggled(
        object sender,
        ToggledEventArgs e)
    {
        ExpiryStack.IsVisible = !e.Value;
    }

    private async void OnPickFileClicked(
        object sender,
        EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(
                new PickOptions
                {
                    PickerTitle = "Select File"
                });

            if (result != null)
            {
                _selectedFilePath = result.FullPath;

                FileInfoBorder.IsVisible = true;

                FileNameLabel.Text = result.FileName;

                PickFileButton.Text = "✅ File Selected";
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

    private async void OnSaveClicked(
        object sender,
        EventArgs e)
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

            if (_certificate == null)
            {
                var cert = new Certificate
                {
                    Document = DocumentEntry.Text,

                    Country = CountryEntry.Text,

                    Number = NumberEntry.Text,

                    IssueDate =
                        Convert.ToDateTime(IssueDatePicker.Date),

                    ExpiryDate = LifetimeSwitch.IsToggled
                        ? DateTime.MaxValue
                        : Convert.ToDateTime(ExpiryDatePicker.Date),

                    IsLifetime =
                        LifetimeSwitch.IsToggled,

                    FilePath = _selectedFilePath,

                    Category = "OTHER"
                };

                _parentPage.AddCertificate(cert);
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
                    Convert.ToDateTime(IssueDatePicker.Date);

                _certificate.ExpiryDate =
                    LifetimeSwitch.IsToggled
                        ? DateTime.MaxValue
                        : Convert.ToDateTime(ExpiryDatePicker.Date);

                _certificate.IsLifetime =
                    LifetimeSwitch.IsToggled;

                _certificate.FilePath =
                    _selectedFilePath;

                _parentPage.AddCertificate(_certificate);
            }

            _mainPage.SetPage(
                new OtherPage(_mainPage));
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private void OnCancelClicked(
        object sender,
        EventArgs e)
    {
        _mainPage.SetPage(
            new OtherPage(_mainPage));
    }
}