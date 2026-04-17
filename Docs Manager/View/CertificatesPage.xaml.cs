using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class CertificatePage : ContentPage
{
    private DatabaseService? _database;
    public ObservableCollection<Certificate> Certificates { get; set; } = new();

    public CertificatePage()
    {
        InitializeComponent();
        CertificateCollectionView.ItemsSource = Certificates;
    }

    private DatabaseService GetDatabase()
    {
        _database ??= ServiceHelper.GetService<DatabaseService>();
        return _database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCertificates();
    }

    private async Task LoadCertificates()
    {
        try
        {
            Certificates.Clear();
            var list = await GetDatabase().GetCertificatesAsync();
            foreach (var cert in list)
                Certificates.Add(cert);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load: {ex.Message}", "OK");
        }
    }

    private async void OnAddCertificateClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddCertificatePage());
    }

    private async void OnEditCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
            await Navigation.PushAsync(new AddCertificatePage(cert));
    }

    private async void OnDeleteCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
        {
            bool answer = await DisplayAlert("Delete", "Delete this certificate?", "Yes", "No");
            if (answer)
            {
                await GetDatabase().DeleteCertificateAsync(cert);
                Certificates.Remove(cert);
            }
        }
    }
}