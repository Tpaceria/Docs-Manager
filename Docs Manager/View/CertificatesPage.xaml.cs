using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class CertificatesPage : ContentPage
{
    private readonly DatabaseService _database;

    public ObservableCollection<Certificate> Certificates { get; set; } = new();

    public CertificatesPage()
    {
        InitializeComponent();

        _database = Application.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetService<DatabaseService>()!;

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Certificates.Clear();
        var list = await _database.GetCertificatesAsync();

        foreach (var cert in list)
            Certificates.Add(cert);
    }

    private async void OnAddClicked(object? sender, EventArgs e)
    {
        var cert = new Certificate
        {
            Name = "Новый сертификат",
            Number = "",
            IssueDate = DateTime.Today,
            ExpiryDate = DateTime.Today.AddMonths(12),
            IsLifetime = false
        };

        await _database.SaveCertificateAsync(cert);

        await Navigation.PushAsync(new EditCertificatePage(cert));
    }

    private async void OnItemTapped(object? sender, EventArgs e)
    {
        if (sender is TapGestureRecognizer tap &&
            tap.CommandParameter is Certificate cert)
        {
            await Navigation.PushAsync(new EditCertificatePage(cert));
        }
    }
}