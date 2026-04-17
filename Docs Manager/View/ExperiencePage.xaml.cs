using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class ExperiencePage : ContentPage
{
    private DatabaseService? _database;
    public ObservableCollection<Experience> Experiences { get; set; } = new();

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

    private async void OnAddExperienceClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditExperiencePage());
    }

    private async void OnEditExperienceClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Experience exp)
            await Navigation.PushAsync(new EditExperiencePage(exp));
    }

    private async void OnDeleteExperienceClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Experience exp)
        {
            bool answer = await DisplayAlert("Delete", "Delete this experience?", "Yes", "No");
            if (answer)
            {
                await GetDatabase().DeleteExperienceAsync(exp);
                Experiences.Remove(exp);
            }
        }
    }
}