using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Docs_Manager.View;

public partial class CertificatePage : ContentPage
{
    private DatabaseService? _database;
    public ObservableCollection<Certificate> Certificates { get; set; } = new();
    private INavigation _navigation;

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