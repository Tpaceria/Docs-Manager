using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class OtherPage : ContentPage
{
    readonly DatabaseService _database;
    ObservableCollection<Certificate> _certificates;

    public OtherPage()
    {
        InitializeComponent();
        _database = Application.Current!.Handler!.MauiContext!.Services.GetService<DatabaseService>()!;
        _certificates = new();
        CertificateCollectionView.ItemsSource = _certificates;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCertificates();
    }

    async Task LoadCertificates()
    {
        var all = await _database.GetCertificatesAsync();
        _certificates.Clear();

        foreach (var cert in all.Where(c => c.Category == "OTHER"))
            _certificates.Add(cert);
    }

    async void OnAddCertificateClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddOtherPage());
    }

    async void OnEditCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
            await Navigation.PushAsync(new AddOtherPage(cert));
    }

    async void OnDeleteCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
        {
            bool answer = await DisplayAlert("Delete", "Delete this certificate?", "Yes", "No");
            if (answer)
            {
                await _database.DeleteCertificateAsync(cert);
                await LoadCertificates();
            }
        }
    }
}