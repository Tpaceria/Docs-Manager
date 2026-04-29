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
    private INavigation _navigation;

    private const string CertificateCategory = "CERTIFICATES";

    public CertificatePage()
    {
        InitializeComponent();
        CertificateCollectionView.ItemsSource = Certificates;
    }

    // ✅ ДОБАВЛЕН: конструктор с INavigation
    public CertificatePage(INavigation navigation) : this()
    {
        _navigation = navigation;
        Debug.WriteLine($"✅ CertificatePage Navigation set: {_navigation != null}");
    }

    private DatabaseService GetDatabase()
    {
        _database ??= ServiceHelper.GetService<DatabaseService>();
        return _database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        MessagingCenter.Subscribe<AddCertificatePage>(this, "CertificateAdded", async (sender) =>
        {
            await LoadCertificates();
        });
        await LoadCertificates();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<AddCertificatePage>(this, "CertificateAdded");
    }

    private async Task LoadCertificates()
    {
        try
        {
            _allCertificates.Clear();
            Certificates.Clear();
            var list = await GetDatabase().GetCertificatesAsync();
            foreach (var cert in list.Where(c => c.Category == CertificateCategory))
                _allCertificates.Add(cert);
            ApplyFilters();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load: {ex.Message}", "OK");
        }
    }

    private void ApplyFilters()
    {
        var query = SearchBar.Text?.Trim() ?? string.Empty;

        IEnumerable<Certificate> filtered = _allCertificates;

        // Search filter
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

        // Update empty message
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

    // ✅ ИСПОЛЬЗУЕМ _navigation вместо this.Navigation
    private async void OnAddCertificateClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("🔵 OnAddCertificateClicked triggered!");
            Debug.WriteLine($"_navigation: {_navigation}");

            if (_navigation == null)
            {
                Debug.WriteLine("❌ Navigation is NULL!");
                await DisplayAlert("Error", "Navigation context is null", "OK");
                return;
            }

            Debug.WriteLine("✅ Starting PushModalAsync...");

            await _navigation.PushModalAsync(
                new NavigationPage(new AddCertificatePage())
                {
                    BarBackgroundColor = Color.FromArgb("#0f1f2e"),
                    BarTextColor = Color.FromArgb("#00d4ff")
                }
            );

            Debug.WriteLine("✅ PushModalAsync completed!");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Exception: {ex.GetType().Name}: {ex.Message}");
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

                await _navigation.PushModalAsync(
                    new NavigationPage(new AddCertificatePage(cert))
                    {
                        BarBackgroundColor = Color.FromArgb("#0f1f2e"),
                        BarTextColor = Color.FromArgb("#00d4ff")
                    }
                );
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