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

        _database = Application.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetService<DatabaseService>()!;

        ExperienceCollectionView.ItemsSource = Experiences;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Experiences.Clear();
        var list = await _database.GetExperiencesAsync();

        foreach (var exp in list)
            Experiences.Add(exp);
    }

    private async void OnAddExperienceClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditExperiencePage());
    }

    private async void OnEditExperienceClicked(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Experience exp)
        {
            await Navigation.PushAsync(new EditExperiencePage(exp));
        }
    }

    private async void OnDeleteExperienceClicked(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Experience exp)
        {
            bool answer = await DisplayAlertAsync("Delete", "Delete this experience?", "Yes", "No");
            if (answer)
            {
                await _database.DeleteExperienceAsync(exp);
                Experiences.Remove(exp);
            }
        }
    }
}