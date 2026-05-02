using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Docs_Manager.View;

public partial class CertificatePage : ContentPage
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
    }

    public CertificatePage(MainPage mainPage) : this()
    {
        _mainPage = mainPage;
        Debug.WriteLine($"✅ CertificatePage MainPage set: {_mainPage != null}");
    }

    // ❗ НЕ полагаемся на OnAppearing
    public async Task LoadCertificatesPublic()
    {
        Debug.WriteLine("🔥 LoadCertificatesPublic CALLED");
        await LoadCertificates();
    }

    private async Task LoadCertificates()
    {
        try
        {
            Debug.WriteLine("🔵 LoadCertificates started!");

            _allCertificates.Clear();
            Certificates.Clear();

            var list = await _database.GetCertificatesAsync();

            Debug.WriteLine($"📊 DB COUNT: {list.Count}");

            // ✅ УБРАЛИ фильтр Category — это была причина бага
            foreach (var cert in list)
            {
                _allCertificates.Add(cert);
            }

            ApplyFilters();

            Debug.WriteLine($"📊 Loaded {_allCertificates.Count} certificates");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ LoadCertificates Error: {ex.Message}");
            await DisplayAlert("Error", $"Failed to load: {ex.Message}", "OK");
        }
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

    private void OnAddCertificateClicked(object sender, EventArgs e)
    {
        var addPage = new AddCertificatePage(this);
        _mainPage?.SetPage(addPage);
    }

    private void OnEditCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
        {
            var addPage = new AddCertificatePage(cert, this);
            _mainPage?.SetPage(addPage);
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
                $"⏰ Expires: {expiryText}";

            await DisplayAlert("Certificate Details", details, "Close");
        }
    }

    private async void OnDeleteCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Certificate cert)
        {
            bool answer = await DisplayAlert(
                "Delete Certificate",
                $"Delete \"{cert.Document}\"?",
                "Delete", "Cancel");

            if (answer)
            {
                await _database.DeleteCertificateAsync(cert);
                _allCertificates.Remove(cert);
                Certificates.Remove(cert);
            }
        }
    }
}