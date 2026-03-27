using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class MedicinePage : ContentPage
{
    private readonly DatabaseService _database;
    public ObservableCollection<Certificate> Certificates { get; set; } = new();

    public MedicinePage()
    {
        InitializeComponent();
        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");
        CertificateCollectionView.ItemsSource = Certificates;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCertificates();
    }

    private async Task LoadCertificates()
    {
        Certificates.Clear();
        var list = await _database.GetCertificatesAsync();
        foreach (var cert in list.Where(c => c.Category == "MEDICINE"))
            Certificates.Add(cert);
    }

    private async void OnAddCertificateClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddMedicinePage());
    }

    private async void OnEditCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
            await Navigation.PushAsync(new AddMedicinePage(cert));
    }

    private async void OnDeleteCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
        {
            bool answer = await DisplayAlert("Delete", "Delete this certificate?", "Yes", "No");
            if (answer)
            {
                await _database.DeleteCertificateAsync(cert);
                Certificates.Remove(cert);
            }
        }
    }
}