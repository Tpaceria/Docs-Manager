using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Docs_Manager.View;

public partial class ExperiencePage : ContentPage
{
    private DatabaseService? _database;
    public ObservableCollection<Experience> Experiences { get; set; } = new();
    private INavigation _navigation;

    public ExperiencePage()
    {
        InitializeComponent();
        ExperienceCollectionView.ItemsSource = Experiences;
    }

    private DatabaseService GetDatabase()
    {
        _database ??= ServiceHelper.GetService<DatabaseService>();
        return _database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadExperiences();
    }

    private async Task LoadExperiences()
    {
        try
        {
            Experiences.Clear();
            var list = await GetDatabase().GetExperiencesAsync();
            foreach (var exp in list)
                Experiences.Add(exp);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load: {ex.Message}", "OK");
        }
    }

    // ✅ ИСПОЛЬЗУЕМ _navigation вместо this.Navigation
    private async void OnAddExperienceClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("🔵 OnAddExperienceClicked triggered!");

            if (_navigation == null)
            {
                Debug.WriteLine("❌ Navigation is NULL!");
                await DisplayAlert("Error", "Navigation context is null", "OK");
                return;
            }

            await _navigation.PushModalAsync(
                new NavigationPage(new EditExperiencePage())
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

    private async void OnEditExperienceClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Experience exp)
        {
            try
            {
                await _navigation.PushModalAsync(
                    new NavigationPage(new EditExperiencePage(exp))
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

    private async void OnDeleteExperienceClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Experience exp)
        {
            bool answer = await DisplayAlert("Delete", "Delete this experience?", "Yes", "No");
            if (answer)
            {
                await _database.DeleteExperienceAsync(exp);
                await LoadExperiences();
            }
        }
    }
}