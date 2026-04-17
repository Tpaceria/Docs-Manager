using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Docs_Manager.View;

public partial class MedicinePage : ContentPage
{
    private DatabaseService? _database;
    public ObservableCollection<Certificate> Certificates { get; set; } = new();
    private INavigation _navigation;

    public MedicinePage()
    {
        InitializeComponent();
        CertificateCollectionView.ItemsSource = Certificates;
    }

    // ✅ КОНСТРУКТОР с INavigation
    public MedicinePage(INavigation navigation) : this()
    {
        _navigation = navigation;
        Debug.WriteLine($"✅ MedicinePage Navigation set: {_navigation != null}");
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
            foreach (var cert in list.Where(c => c.Category == "MEDICINE"))
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

            if (_navigation == null)
            {
                Debug.WriteLine("❌ Navigation is NULL!");
                await DisplayAlert("Error", "Navigation context is null", "OK");
                return;
            }

            await _navigation.PushModalAsync(
                new NavigationPage(new AddMedicinePage())
                {
                    BarBackgroundColor = Color.FromArgb("#0f1f2e"),
                    BarTextColor = Color.FromArgb("#00d4ff")
                }
            );
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
                await _navigation.PushModalAsync(
                    new NavigationPage(new AddMedicinePage(cert))
                    {
                        BarBackgroundColor = Color.FromArgb("#0f1f2e"),
                        BarTextColor = Color.FromArgb("#00d4ff")
                    }
                );
            }
            catch (Exception ex)
            {
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