using Docs_Manager.Data;

namespace Docs_Manager.View;

public partial class PersonalPage : ContentView
{
    private DatabaseService? _database;

    private string? _photoPath;

    private bool _isEditMode = false;

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

        await LoadCocPreview();
        await LoadCertificatesPreview();
        await LoadExperiencePreview();

        await LoadDocumentsPreview();

        await LoadSkillsPreview();
        await LoadBiometricPreview();
        await LoadNextOfKinPreview();
    }

    // ===== EDIT MODE TOGGLE =====

    private void OnEditMainProfileClicked(object sender, EventArgs e)
    {
        if (_isEditMode)
        {
            // Save mode - exit edit
            ExitEditMode();
        }
        else
        {
            // Enter edit mode
            EnterEditMode();
        }
    }

    private void EnterEditMode()
    {
        _isEditMode = true;

        // Hide EDIT button
        EditButton.IsVisible = false;

        // Show photo and save buttons
        PickPhotoButton.IsVisible = true;
        SaveButton.IsVisible = true;

        // Hide Labels and show Entries
        FirstNameLabel.IsVisible = false;
        FirstNameEntry.IsVisible = true;
        FirstNameEntry.IsEnabled = true;

        MiddleNameLabel.IsVisible = false;
        MiddleNameEntry.IsVisible = true;
        MiddleNameEntry.IsEnabled = true;

        LastNameLabel.IsVisible = false;
        LastNameEntry.IsVisible = true;
        LastNameEntry.IsEnabled = true;

        NationalityLabel.IsVisible = false;
        NationalityEntry.IsVisible = true;
        NationalityEntry.IsEnabled = true;

        CitizenshipLabel.IsVisible = false;
        CitizenshipEntry.IsVisible = true;
        CitizenshipEntry.IsEnabled = true;

        PlaceOfBirthLabel.IsVisible = false;
        PlaceOfBirthEntry.IsVisible = true;
        PlaceOfBirthEntry.IsEnabled = true;

        BirthDateLabel.IsVisible = false;
        BirthDatePicker.IsVisible = true;
        BirthDatePicker.IsEnabled = true;

        GenderLabel.IsVisible = false;
        GenderPicker.IsVisible = true;
        GenderPicker.IsEnabled = true;

        // Show all Edit buttons in right column
        EditContactsButton.IsVisible = true;
        EditEducationButton.IsVisible = true;
        EditSkillsButton.IsVisible = true;
        EditBiometricButton.IsVisible = true;
        EditNextOfKinButton.IsVisible = true;
    }

    private void ExitEditMode()
    {
        _isEditMode = false;

        // Show EDIT button
        EditButton.IsVisible = true;

        // Hide photo and save buttons
        PickPhotoButton.IsVisible = false;
        SaveButton.IsVisible = false;

        // Show Labels and hide Entries
        FirstNameLabel.IsVisible = true;
        FirstNameEntry.IsVisible = false;
        FirstNameEntry.IsEnabled = false;

        MiddleNameLabel.IsVisible = true;
        MiddleNameEntry.IsVisible = false;
        MiddleNameEntry.IsEnabled = false;

        LastNameLabel.IsVisible = true;
        LastNameEntry.IsVisible = false;
        LastNameEntry.IsEnabled = false;

        NationalityLabel.IsVisible = true;
        NationalityEntry.IsVisible = false;
        NationalityEntry.IsEnabled = false;

        CitizenshipLabel.IsVisible = true;
        CitizenshipEntry.IsVisible = false;
        CitizenshipEntry.IsEnabled = false;

        PlaceOfBirthLabel.IsVisible = true;
        PlaceOfBirthEntry.IsVisible = false;
        PlaceOfBirthEntry.IsEnabled = false;

        BirthDateLabel.IsVisible = true;
        BirthDatePicker.IsVisible = false;
        BirthDatePicker.IsEnabled = false;

        GenderLabel.IsVisible = true;
        GenderPicker.IsVisible = false;
        GenderPicker.IsEnabled = false;

        // Hide all Edit buttons in right column
        EditContactsButton.IsVisible = false;
        EditEducationButton.IsVisible = false;
        EditSkillsButton.IsVisible = false;
        EditBiometricButton.IsVisible = false;
        EditNextOfKinButton.IsVisible = false;
    }

    private async void OnEditDocumentsClicked(object sender, EventArgs e)
    {
        var mainPage = Application.Current.MainPage as MainPage;

        if (mainPage != null)
        {
            mainPage.SetPage(new DocumentsPage(mainPage));
        }
    }
}
