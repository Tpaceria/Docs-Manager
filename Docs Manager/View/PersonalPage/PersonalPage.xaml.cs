using Docs_Manager.Data;

namespace Docs_Manager.View;

public partial class PersonalPage : ContentView
{
    private DatabaseService? _database;

    private string? _photoPath;

    public PersonalPage()
    {
        InitializeComponent();

        BirthDatePicker.Date =
            DateTime.Today;

        _ = LoadProfileAsync();

        Loaded += PersonalPage_Loaded;
    }

    private DatabaseService GetDatabase()
    {
        _database ??=
            ServiceHelper.GetService<DatabaseService>();

        return _database!;
    }

    private async void PersonalPage_Loaded(
        object? sender,
        EventArgs e)
    {
        await LoadProfileAsync();

        await LoadContactsPreview();

        await LoadEducationPreview();

        await LoadVisaPreview();

        await LoadSkillsPreview();

        await LoadBiometricPreview();

        await LoadNextOfKinPreview();
    }
}