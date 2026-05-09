using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class ExperiencePage : ContentPage
{
    private DatabaseService _database;

    public ObservableCollection<Experience> Experiences { get; set; } = new();

    private MainPage _mainPage;

    public ExperiencePage()
    {
        InitializeComponent();

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        ExperienceCollectionView.ItemsSource = Experiences;

        _ = LoadExperiences();
    }

    public ExperiencePage(MainPage mainPage) : this()
    {
        _mainPage = mainPage;
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

        foreach (var exp in list.OrderByDescending(x => x.SignOffDate))
        {
            Experiences.Add(exp);
        }
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        var page = new EditExperiencePage(this, _mainPage);

        _mainPage.SetPage(page);
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Experience exp)
        {
            var page =
                new EditExperiencePage(exp, this, _mainPage);

            _mainPage.SetPage(page);
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Experience exp)
        {
            bool confirm =
                await Application.Current.Windows[0].Page.DisplayAlertAsync(
                    "Delete",
                    $"Delete experience on {exp.VesselName}?",
                    "Yes",
                    "Cancel");

            if (!confirm)
                return;

            await _database.DeleteExperienceAsync(exp);

            await LoadExperiences();
        }
    }

    public async void RefreshList()
    {
        await LoadExperiences();
    }
}