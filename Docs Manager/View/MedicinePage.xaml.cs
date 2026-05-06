using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Docs_Manager.View;

public partial class MedicinePage : ContentPage
{
    private DatabaseService _database;
    private ObservableCollection<Certificate> _allCertificates = new();

    public ObservableCollection<Certificate> Certificates { get; set; } = new();

    private MainPage _mainPage;

    public MedicinePage()
    {
        InitializeComponent();

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        CertificateCollectionView.ItemsSource = Certificates;
        _ = LoadCertificates();
    }

    public MedicinePage(MainPage mainPage) : this()
    {
        _mainPage = mainPage;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadCertificates();
    }

    private async Task LoadCertificates()
    {
        _allCertificates.Clear();
        Certificates.Clear();

        var list = await _database.GetCertificatesAsync();

        foreach (var cert in list.Where(c => c.Category == "MEDICINE"))
        {
            _allCertificates.Add(cert);
        }
        foreach (var cert in _allCertificates)
        {
            Certificates.Add(cert);
        }
    }

    private void OnAddCertificateClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("➕ ADD MEDICINE");

        var page = new AddMedicinePage(this, _mainPage);

        _mainPage.SetPage(page);
    }

    private void OnEditCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Certificate cert)
        {
            Debug.WriteLine($"✏️ EDIT MEDICINE: {cert.Document}");

            var page = new AddMedicinePage(cert, this, _mainPage);

            _mainPage.SetPage(page);
        }
    }

    private async void OnDeleteCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Certificate cert)
        {
            bool confirm = await DisplayAlert(
                "Delete",
                $"Delete \"{cert.Document}\"?",
                "Yes",
                "Cancel");

            if (!confirm)
                return;

            await _database.DeleteAsync(cert);

            await LoadCertificates();
        }
    }

    public async void AddCertificate(Certificate cert)
    {
        await _database.SaveCertificateAsync(cert);

        await LoadCertificates();
    }

    public async void RefreshList()
    {
        await LoadCertificates();
    }
}