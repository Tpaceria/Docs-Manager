using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class ExperiencePage : ContentPage
{
    private readonly DatabaseService _database;

    public ObservableCollection<ExperienceViewModel> Experiences { get; set; } = new();

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
        {
            Experiences.Add(new ExperienceViewModel(exp));
        }
    }

    private async void OnAddExperienceClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditExperiencePage());
    }

    private async void OnEditExperienceClicked(object? sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ExperienceViewModel viewModel)
        {
            await Navigation.PushAsync(new EditExperiencePage(viewModel.Model));
        }
    }

    private async void OnDeleteExperienceClicked(object? sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ExperienceViewModel viewModel)
        {
            bool answer = await DisplayAlert("Delete", "Delete this experience?", "Yes", "No");
            if (answer)
            {
                await _database.DeleteExperienceAsync(viewModel.Model);
                Experiences.Remove(viewModel);
            }
        }
    }
}

// ViewModel для вычисления Duration
public class ExperienceViewModel
{
    public Experience Model { get; }

    public ExperienceViewModel(Experience model) => Model = model;

    public string VesselName => Model.VesselName;
    public string Position => Model.Position;
    public string VesselType => Model.VesselType;
    public int DWT => Model.DWT;
    public string Flag => Model.Flag;
    public int YearOfBuilt => Model.YearOfBuilt;
    public DateTime SignOnDate => Model.SignOnDate;
    public DateTime SignOffDate => Model.SignOffDate;
    public string Shipowner => Model.Shipowner;
    public string CrewingAgency => Model.CrewingAgency;
    public int MainEngineKW => Model.MainEngineKW;
    public string METype => Model.METype;
    public string IMO => Model.IMO;
    public bool IsVessel => Model.IsVessel;

    public string Duration
    {
        get
        {
            var diff = SignOffDate - SignOnDate;
            int months = (int)(diff.TotalDays / 30.44);
            int years = months / 12;
            months = months % 12;

            if (years > 0)
                return $"{years}y {months}m";
            else
                return $"{months}m";
        }
    }
}