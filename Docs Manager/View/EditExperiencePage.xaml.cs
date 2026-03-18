using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class EditExperiencePage : ContentPage
{
    private readonly DatabaseService _database;
    private Experience? _experience;

    public EditExperiencePage()
    {
        InitializeComponent();
        _database = Application.Current!.Handler!.MauiContext!.Services.GetService<DatabaseService>()!;
    }

    public EditExperiencePage(Experience experience) : this()
    {
        _experience = experience;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_experience != null)
        {
            VesselNameEntry.Text = _experience.VesselName;
            DWTEntry.Text = _experience.DWT.ToString();
            PositionPicker.SelectedItem = _experience.Position;
            VesselTypePicker.SelectedItem = _experience.VesselType;
            FlagPicker.SelectedItem = _experience.Flag;
            YearEntry.Text = _experience.YearOfBuilt.ToString();
            SignOnDatePicker.Date = _experience.SignOnDate;
            SignOffDatePicker.Date = _experience.SignOffDate;
            ShipownerEntry.Text = _experience.Shipowner;
            CrewingAgencyEntry.Text = _experience.CrewingAgency;
            MainEngineEntry.Text = _experience.MainEngineKW.ToString();
            METypePicker.SelectedItem = _experience.METype;
            IMOEntry.Text = _experience.IMO;

            Title = "✏️ Edit Experience";
        }
        else
        {
            Title = "➕ Add Experience";
            SignOnDatePicker.Date = DateTime.Today;
            SignOffDatePicker.Date = DateTime.Today;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(VesselNameEntry.Text))
            {
                await DisplayAlert("Error", "Enter vessel name", "OK");
                return;
            }

            int.TryParse(DWTEntry.Text, out int dwt);
            int.TryParse(YearEntry.Text, out int year);
            int.TryParse(MainEngineEntry.Text, out int mainEngine);

            var experience = new Experience
            {
                Id = _experience?.Id ?? 0,
                VesselName = VesselNameEntry.Text,
                DWT = dwt,
                Position = PositionPicker.SelectedItem?.ToString(),
                VesselType = VesselTypePicker.SelectedItem?.ToString(),
                Flag = FlagPicker.SelectedItem?.ToString(),
                YearOfBuilt = year,
                SignOnDate = SignOnDatePicker.Date ?? DateTime.Today,
                SignOffDate = SignOffDatePicker.Date ?? DateTime.Today,
                Shipowner = ShipownerEntry.Text,
                CrewingAgency = CrewingAgencyEntry.Text,
                MainEngineKW = mainEngine,
                METype = METypePicker.SelectedItem?.ToString(),
                IMO = IMOEntry.Text,
                IsVessel = true
            };

            if (_experience == null)
                await _database.SaveExperienceAsync(experience);
            else
                await _database.UpdateExperienceAsync(experience);

            await DisplayAlert("Success", "Experience saved!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error saving: {ex.Message}", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}