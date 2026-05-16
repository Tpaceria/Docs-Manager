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
            SignOnDatePicker.Date = DateTime.Today;

            SignOffDatePicker.Date = DateTime.Today;
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

        SignOnDatePicker.Date =
            Convert.ToDateTime(_experience.SignOnDate);

        SignOffDatePicker.Date =
            Convert.ToDateTime(_experience.SignOffDate);
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

                SignOnDate =
                    Convert.ToDateTime(SignOnDatePicker.Date),

                SignOffDate =
                    Convert.ToDateTime(SignOffDatePicker.Date)
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