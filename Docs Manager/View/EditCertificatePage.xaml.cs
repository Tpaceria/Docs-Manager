using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class EditCertificatePage : ContentPage
{
    private readonly DatabaseService _database;
    private readonly Certificate _certificate;

    public EditCertificatePage(Certificate certificate)
    {
        InitializeComponent();

        _database = Application.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetService<DatabaseService>()!;

        _certificate = certificate;

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
        _certificate.IssueDate = IssueDatePicker.Date ?? DateTime.Today;
        _certificate.ExpiryDate = ExpiryDatePicker.Date ?? DateTime.Today;
        _certificate.IsLifetime = LifetimeSwitch.IsToggled;

        await _database.SaveCertificateAsync(_certificate);
        await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        await _database.DeleteCertificateAsync(_certificate);
        await Navigation.PopAsync();
    }
}