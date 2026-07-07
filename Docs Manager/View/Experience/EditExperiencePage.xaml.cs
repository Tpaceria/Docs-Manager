using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class EditExperiencePage : ContentView
{
    private readonly DatabaseService _database;

    private readonly ExperiencePage _parentPage;

    private readonly MainPage _mainPage;

    private Experience _experience;

    public EditExperiencePage(
        ExperiencePage parentPage,
        MainPage mainPage)
    {
        InitializeComponent();

        _parentPage = parentPage;

        _mainPage = mainPage;

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        if (_experience == null)
        {
            SignOnDateControl.SelectedDate = DateTime.Today;
            SignOffDateControl.SelectedDate = DateTime.Today;
        }
    }

    public EditExperiencePage(
        Experience experience,
        ExperiencePage parentPage,
        MainPage mainPage)
        : this(parentPage, mainPage)
    {
        _experience = experience;

        FillForm();
    }

    private void FillForm()
    {
        if (_experience == null)
            return;

        VesselNameEntry.Text = _experience.VesselName;

        DWTEntry.Text = _experience.DWT.ToString();

        PositionPicker.SelectedItem = _experience.Position;

        VesselTypePicker.SelectedItem = _experience.VesselType;

        FlagPicker.SelectedItem = _experience.Flag;

        YearEntry.Text = _experience.YearOfBuilt.ToString();

        SignOnDateControl.SelectedDate = _experience.SignOnDate;

        SignOffDateControl.SelectedDate = _experience.SignOffDate;

        MainEngineEntry.Text = _experience.MainEngineKW.ToString();

        METypePicker.SelectedItem = _experience.METype;

        ShipownerEntry.Text = _experience.Shipowner;

        CrewingAgencyEntry.Text = _experience.CrewingAgency;

        IMOEntry.Text = _experience.IMO;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(VesselNameEntry.Text))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Enter vessel name",
                    "OK");

                return;
            }

            var exp = new Experience
            {
                Id = _experience?.Id ?? 0,

                VesselName = VesselNameEntry.Text,

                DWT = int.TryParse(DWTEntry.Text, out int dwt)
                    ? dwt
                    : 0,

                Position = PositionPicker.SelectedItem?.ToString() ?? "",

                VesselType = VesselTypePicker.SelectedItem?.ToString(),

                Flag = FlagPicker.SelectedItem?.ToString(),

                YearOfBuilt = int.TryParse(YearEntry.Text, out int year)
    ? year
    : 0,

                SignOnDate = SignOnDateControl.SelectedDate,

                SignOffDate = SignOffDateControl.SelectedDate,

                MainEngineKW = int.TryParse(MainEngineEntry.Text, out int kw)
    ? kw
    : 0,
                METype = METypePicker.SelectedItem?.ToString(),

                Shipowner = ShipownerEntry.Text,

                CrewingAgency = CrewingAgencyEntry.Text,

                IMO = IMOEntry.Text
            };
            await _database.SaveExperienceAsync(exp);

            _parentPage.RefreshList();

            _mainPage.SetPage(new ExperiencePage(_mainPage));
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        _mainPage.SetPage(new ExperiencePage(_mainPage));
    }
}