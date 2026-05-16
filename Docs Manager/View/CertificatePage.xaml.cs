using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Docs_Manager.View;

public partial class CertificatePage : ContentView
{
    private DatabaseService _database;
    private ObservableCollection<Certificate> _allCertificates = new();

    public ObservableCollection<Certificate> Certificates { get; set; } = new();

    private MainPage _mainPage;

    public CertificatePage()
    {
        InitializeComponent();

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        CertificateCollectionView.ItemsSource = Certificates;

        _ = LoadCertificates();
    }

    public CertificatePage(MainPage mainPage)
    {
        InitializeComponent();

        _mainPage = mainPage;

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        CertificateCollectionView.ItemsSource = Certificates;

        _ = LoadCertificates();
    }

    public async Task LoadCertificatesPublic()
    {
        await LoadCertificates();
    }

    private async Task LoadCertificates()
    {
        _allCertificates.Clear();
        Certificates.Clear();

        var list = await _database.GetCertificatesAsync();

        foreach (var cert in list.Where(c => c.Category == "CERTIFICATES"))
        {
            _allCertificates.Add(cert);
        }

        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var query = SearchBar.Text?.Trim() ?? string.Empty;

        IEnumerable<Certificate> filtered = _allCertificates;

        if (!string.IsNullOrWhiteSpace(query))
        {
            var lower = query.ToLowerInvariant();

            filtered = filtered.Where(c =>
                c.Document.ToLowerInvariant().Contains(lower) ||
                (c.Country ?? "").ToLowerInvariant().Contains(lower) ||
                c.Number.ToLowerInvariant().Contains(lower));
        }

        Certificates.Clear();

        foreach (var cert in filtered)
        {
            Certificates.Add(cert);
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        await LoadCertificates();

        CertificateRefreshView.IsRefreshing = false;
    }

    private void OnAddCertificateClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("➕ ADD clicked");

        var addPage = new AddCertificatePage(this, _mainPage);

        _mainPage.SetPage(addPage);
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Certificate cert)
        {
            Debug.WriteLine($"✏️ EDIT clicked: {cert.Document}");

            var addPage =
                new AddCertificatePage(cert, this, _mainPage);

            _mainPage.SetPage(addPage);
        }
    }

    private async void OnViewCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Certificate cert)
        {
            Debug.WriteLine($"👁 VIEW clicked: {cert.Document}");

            if (string.IsNullOrWhiteSpace(cert.FilePath) ||
                !File.Exists(cert.FilePath))
            {
                bool attachNow =
                    await Application.Current.MainPage.DisplayAlert(
                        "Файл не найден",
                        "У сертификата нет прикрепленного файла.\nПрикрепить сейчас?",
                        "Прикрепить",
                        "Отмена");

                if (!attachNow)
                    return;

                var result = await FilePicker.PickAsync();

                if (result != null)
                {
                    cert.FilePath = result.FullPath;

                    await _database.SaveCertificateAsync(cert);

                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(cert.FilePath)
                    });
                }

                return;
            }

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(cert.FilePath)
            });
        }
    }

    private async void OnDeleteCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Certificate cert)
        {
            Debug.WriteLine($"🗑 DELETE clicked: {cert.Document}");

            bool confirm =
                await Application.Current.MainPage.DisplayAlert(
                    "Delete Certificate",
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