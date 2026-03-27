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
            DWTEntry.Text = _experience.DWT > 0 ? _experience.DWT.ToString() : "";

            if (PositionPicker.Items.Count > 0 && !string.IsNullOrEmpty(_experience.Position))
            {
                var index = PositionPicker.Items.IndexOf(_experience.Position);
                if (index >= 0) PositionPicker.SelectedIndex = index;
            }

            if (VesselTypePicker.Items.Count > 0 && !string.IsNullOrEmpty(_experience.VesselType))
            {
                var index = VesselTypePicker.Items.IndexOf(_experience.VesselType);
                if (index >= 0) VesselTypePicker.SelectedIndex = index;
            }

            if (FlagPicker.Items.Count > 0 && !string.IsNullOrEmpty(_experience.Flag))
            {
                var index = FlagPicker.Items.IndexOf(_experience.Flag);
                if (index >= 0) FlagPicker.SelectedIndex = index;
            }

            YearEntry.Text = _experience.YearOfBuilt > 0 ? _experience.YearOfBuilt.ToString() : "";

            SignOnDatePicker.Date = _experience.SignOnDate;
            SignOffDatePicker.Date = _experience.SignOffDate;

            MainEngineEntry.Text = _experience.MainEngineKW > 0 ? _experience.MainEngineKW.ToString() : "";

            if (METypePicker.Items.Count > 0 && !string.IsNullOrEmpty(_experience.METype))
            {
                var index = METypePicker.Items.IndexOf(_experience.METype);
                if (index >= 0) METypePicker.SelectedIndex = index;
            }

            ShipownerEntry.Text = _experience.Shipowner ?? "";
            CrewingAgencyEntry.Text = _experience.CrewingAgency ?? "";
            IMOEntry.Text = _experience.IMO ?? "";

            Title = "Edit Experience";
        }
        else
        {
            SignOnDatePicker.Date = DateTime.Today;
            SignOffDatePicker.Date = DateTime.Today;
            PositionPicker.SelectedIndex = 0;
            VesselTypePicker.SelectedIndex = 0;
            FlagPicker.SelectedIndex = 0;
            METypePicker.SelectedIndex = 0;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(VesselNameEntry.Text) || PositionPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Error", "Please fill required fields", "OK");
            return;
        }

        try
        {
            int.TryParse(DWTEntry.Text, out int dwt);
            int.TryParse(YearEntry.Text, out int year);
            int.TryParse(MainEngineEntry.Text, out int mainEngine);

            var experience = new Experience
            {
                Id = _experience?.Id ?? 0,
                VesselName = VesselNameEntry.Text,
                DWT = dwt,
                Position = PositionPicker.SelectedItem?.ToString() ?? "",
                VesselType = VesselTypePicker.SelectedItem?.ToString() ?? "",
                Flag = FlagPicker.SelectedItem?.ToString() ?? "",
                YearOfBuilt = year,
                SignOnDate = SignOnDatePicker.Date ?? DateTime.Today,
                SignOffDate = SignOffDatePicker.Date ?? DateTime.Today,
                MainEngineKW = mainEngine,
                METype = METypePicker.SelectedItem?.ToString() ?? "",
                Shipowner = ShipownerEntry.Text ?? "",
                CrewingAgency = CrewingAgencyEntry.Text ?? "",
                IMO = IMOEntry.Text ?? ""
            };

            await _database.SaveExperienceAsync(experience);
            await DisplayAlert("Success", "Experience saved!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}