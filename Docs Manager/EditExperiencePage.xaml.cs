using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class EditExperiencePage : ContentPage
{
    readonly DatabaseService _database;
    Experience _experience;

    public EditExperiencePage()
    {
        InitializeComponent();
        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");
    }

    public EditExperiencePage(Experience experience) : this()
    {
        _experience = experience;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_experience != null)
        {
            VesselNameEntry.Text = _experience.VesselName ?? "";
            DWTEntry.Text = _experience.DWT.ToString();
            PositionPicker.SelectedItem = _experience.Position;
            VesselTypePicker.SelectedItem = _experience.VesselType;
            FlagPicker.SelectedItem = _experience.Flag;
            YearEntry.Text = _experience.YearOfBuilt.ToString();
            SignOnDatePicker.Date = _experience.SignOnDate;
            SignOffDatePicker.Date = _experience.SignOffDate;
            MainEngineEntry.Text = _experience.MainEngineKW.ToString();
            METypePicker.SelectedItem = _experience.METype;
            ShipownerEntry.Text = _experience.Shipowner ?? "";
            CrewingAgencyEntry.Text = _experience.CrewingAgency ?? "";
            IMOEntry.Text = _experience.IMO ?? "";

            Title = "Edit Experience";
        }
        else
        {
            SignOnDatePicker.Date = DateTime.Today;
            SignOffDatePicker.Date = DateTime.Today;
        }
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(VesselNameEntry.Text))
            {
                await DisplayAlert("Error", "Enter vessel name", "OK");
                return;
            }

            var experience = new Experience
            {
                Id = _experience?.Id ?? 0,
                VesselName = VesselNameEntry.Text,
                DWT = int.TryParse(DWTEntry.Text, out int dwt) ? dwt : 0,
                Position = PositionPicker.SelectedItem?.ToString() ?? "",
                VesselType = VesselTypePicker.SelectedItem?.ToString() ?? "",
                Flag = FlagPicker.SelectedItem?.ToString() ?? "",
                YearOfBuilt = int.TryParse(YearEntry.Text, out int year) ? year : 0,
                SignOnDate = SignOnDatePicker.Date ?? DateTime.Today,
                SignOffDate = SignOffDatePicker.Date ?? DateTime.Today,
                MainEngineKW = int.TryParse(MainEngineEntry.Text, out int engine) ? engine : 0,
                METype = METypePicker.SelectedItem?.ToString() ?? "",
                Shipowner = ShipownerEntry.Text ?? "",
                CrewingAgency = CrewingAgencyEntry.Text ?? "",
                IMO = IMOEntry.Text ?? ""
            };

            await _database.SaveExperienceAsync(experience);
            await DisplayAlert("Success", "Experience saved!", "OK");
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}