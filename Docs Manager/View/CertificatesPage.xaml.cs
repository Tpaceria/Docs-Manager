using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Docs_Manager.View;

public partial class CertificatePage : ContentPage
{
    private DatabaseService? _database;
    private ObservableCollection<Certificate> _allCertificates = new();
    public ObservableCollection<Certificate> Certificates { get; set; } = new();
    private MainPage _mainPage;  // ← ИЗМЕНЕНО с INavigation на MainPage

    private const string CertificateCategory = "CERTIFICATES";

    public CertificatePage()
    {
        InitializeComponent();
        CertificateCollectionView.ItemsSource = Certificates;
    }

    // ✅ КОНСТРУКТОР С MainPage
    public CertificatePage(MainPage mainPage) : this()
    {
        _mainPage = mainPage;
        Debug.WriteLine($"✅ CertificatePage MainPage set: {_mainPage != null}");
    }

    private DatabaseService GetDatabase()
    {
        _database ??= ServiceHelper.GetService<DatabaseService>();
        return _database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine("🔵 CertificatePage OnAppearing!");
        await Task.Delay(100);
        await LoadCertificates();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private async Task LoadCertificates()
    {
        try
        {
            Debug.WriteLine("🔵 LoadCertificates started!");
            _allCertificates.Clear();
            Certificates.Clear();
            var list = await GetDatabase().GetCertificatesAsync();
            Debug.WriteLine($"📊 Got {list.Count} total certificates from DB");

            foreach (var cert in list.Where(c => c.Category == CertificateCategory))
            {
                _allCertificates.Add(cert);
                Debug.WriteLine($"   ✅ Added: {cert.Document}");
            }

            Debug.WriteLine($"📊 Loaded {_allCertificates.Count} CERTIFICATES");
            ApplyFilters();
            Debug.WriteLine($"📊 After filters: {Certificates.Count} visible");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ LoadCertificates Error: {ex.Message}");
            await DisplayAlert("Error", $"Failed to load: {ex.Message}", "OK");
        }
    }

    // ✅ PUBLIC метод для вызова из AddCertificatePage
    public async Task LoadCertificatesPublic()
    {
        await LoadCertificates();
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
                (c.Country ?? string.Empty).ToLowerInvariant().Contains(lower) ||
                c.Number.ToLowerInvariant().Contains(lower));
        }

        Certificates.Clear();
        foreach (var cert in filtered)
            Certificates.Add(cert);

        EmptyLabel.Text = string.IsNullOrWhiteSpace(query)
            ? "No certificates yet.\nTap ➕ to add one."
            : "No certificates match your search.";
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

    private async void OnAddCertificateClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("🔵 OnAddCertificateClicked triggered!");

            var addPage = new AddCertificatePage(_mainPage, this);
            _mainPage?.SetPage(addPage);

            Debug.WriteLine($"✅ SetPage completed!");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Exception: {ex.Message}");
            await DisplayAlert("Error", $"Failed: {ex.Message}", "OK");
        }
    }

    private async void OnEditCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
        {
            try
            {
                Debug.WriteLine($"✏️ Editing certificate: {cert.Document}");
                var addPage = new AddCertificatePage(cert, _mainPage, this);
                _mainPage?.SetPage(addPage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Edit Error: {ex.Message}");
                await DisplayAlert("Error", $"Failed: {ex.Message}", "OK");
            }
        }
    }

    private async void OnViewCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
        {
            var expiryText = cert.IsLifetime
                ? "No expiration date"
                : cert.ExpiryDate.ToString("dd MMM yyyy");

            var details =
                $"📋 Name: {cert.Document}\n" +
                $"🌍 Country: {cert.Country ?? "N/A"}\n" +
                $"🔢 Number: {cert.Number}\n" +
                $"📅 Issued: {cert.IssueDate:dd MMM yyyy}\n" +
                $"⏰ Expires: {expiryText}\n" +
                $"📊 Status: {cert.StatusDisplay}\n" +
                $"⏳ {cert.DaysRemainingDisplay}";

            await DisplayAlert("Certificate Details", details, "Close");
        }
    }

    private async void OnDeleteCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
        {
            bool answer = await DisplayAlert(
                "Delete Certificate",
                $"Are you sure you want to delete \"{cert.Document}\"?",
                "Delete", "Cancel");

            if (answer)
            {
                await GetDatabase().DeleteCertificateAsync(cert);
                _allCertificates.Remove(cert);
                Certificates.Remove(cert);
            }
        }
    }
}