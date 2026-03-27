using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class ExperiencePage : ContentPage
{
    private readonly DatabaseService _database;
    public ObservableCollection<Experience> Experiences { get; set; } = new();

    public ExperiencePage()
    {
        InitializeComponent();
        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");
        ExperienceCollectionView.ItemsSource = Experiences;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadExperiences();
    }

    private async Task LoadExperiences()
    {
        Experiences.Clear();
        var list = await _database.GetExperiencesAsync();
        foreach (var exp in list)
            Experiences.Add(exp);
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
                await _database.DeleteExperienceAsync(exp);
                await LoadExperiences();
            }
        }
    }
}