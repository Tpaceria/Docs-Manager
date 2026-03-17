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

        _database = Application.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetService<DatabaseService>()!;

        // Заполни года
        var years = Enumerable.Range(1980, DateTime.Now.Year - 1980 + 1)
                              .Select(y => y.ToString())
                              .Reverse()
                              .ToList();
        YearPicker.ItemsSource = years;

        SignOnDatePicker.Date = DateTime.Today;
        SignOffDatePicker.Date = DateTime.Today.AddMonths(1);
    }

    // Перегруженный конструктор для редактирования
    public EditExperiencePage(Experience experience) : this()
    {
        _experience = experience;

        if (experience != null)
        {
            Title = "Edit Trip";
            VesselNameEntry.Text = experience.VesselName;
            DWTEntry.Text = experience.DWT.ToString();
            PositionPicker.SelectedItem = experience.Position;
            VesselTypePicker.SelectedItem = experience.VesselType;
            FlagPicker.SelectedItem = experience.Flag;
            YearPicker.SelectedItem = experience.YearOfBuilt.ToString();
            SignOnDatePicker.Date = experience.SignOnDate;
            SignOffDatePicker.Date = experience.SignOffDate;
            ShipownerEntry.Text = experience.Shipowner;
            CrewingAgencyEntry.Text = experience.CrewingAgency;
            MainEngineEntry.Text = experience.MainEngineKW.ToString();
            METypePicker.SelectedItem = experience.METype;
            IMOEntry.Text = experience.IMO;
            VesselRadio.IsChecked = experience.IsVessel;
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        var exp = _experience ?? new Experience();

        exp.VesselName = VesselNameEntry.Text ?? "";
        exp.DWT = int.TryParse(DWTEntry.Text, out var dwt) ? dwt : 0;
        exp.Position = PositionPicker.SelectedItem?.ToString() ?? "";
        exp.VesselType = VesselTypePicker.SelectedItem?.ToString() ?? "";
        exp.Flag = FlagPicker.SelectedItem?.ToString() ?? "";
        exp.YearOfBuilt = int.TryParse(YearPicker.SelectedItem?.ToString(), out var year) ? year : 0;

        // ✅ ИСПРАВЛЕНО: Преобразование DateTime?
        exp.SignOnDate = SignOnDatePicker.Date ?? DateTime.Today;
        exp.SignOffDate = SignOffDatePicker.Date ?? DateTime.Today;

        exp.Shipowner = ShipownerEntry.Text ?? "";
        exp.CrewingAgency = CrewingAgencyEntry.Text ?? "";
        exp.MainEngineKW = int.TryParse(MainEngineEntry.Text, out var kw) ? kw : 0;
        exp.METype = METypePicker.SelectedItem?.ToString() ?? "";
        exp.IMO = IMOEntry.Text ?? "";
        exp.IsVessel = VesselRadio.IsChecked;

        await _database.SaveExperienceAsync(exp);
        await Navigation.PopAsync();
    }
    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}