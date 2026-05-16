using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class EditCertificatePage : ContentView
{
    private readonly DatabaseService _database;

    private readonly Certificate _certificate;

    private readonly CertificatePage _parentPage;

    private readonly MainPage _mainPage;

    public EditCertificatePage(
        Certificate certificate,
        CertificatePage parentPage,
        MainPage mainPage)
    {
        InitializeComponent();

        _database = Application.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetService<DatabaseService>()!;

        _certificate = certificate;

        _parentPage = parentPage;

        _mainPage = mainPage;

        NameEntry.Text = certificate.Document;

        NumberEntry.Text = certificate.Number;

        IssueDatePicker.Date = certificate.IssueDate;

        ExpiryDatePicker.Date = certificate.ExpiryDate;

        LifetimeSwitch.IsToggled = certificate.IsLifetime;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _certificate.Document = NameEntry.Text ?? "";

        _certificate.Number = NumberEntry.Text ?? "";

        _certificate.IssueDate =
            Convert.ToDateTime(IssueDatePicker.Date);
        _certificate.ExpiryDate =
            Convert.ToDateTime(ExpiryDatePicker.Date);
        _certificate.IsLifetime = LifetimeSwitch.IsToggled;

        await _database.SaveCertificateAsync(_certificate);

        _parentPage.RefreshList();

        _mainPage.SetPage(
            new CertificatePage(_mainPage));
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        await _database.DeleteCertificateAsync(_certificate);

        _parentPage.RefreshList();

        _mainPage.SetPage(
            new CertificatePage(_mainPage));
    }
}