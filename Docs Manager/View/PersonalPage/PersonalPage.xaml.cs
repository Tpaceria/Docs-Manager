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
            // Save mode - hide edit elements
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

        // Change button text and color
        EditButton.Text = "✏️ EDITING...";
        EditButton.BackgroundColor = Color.FromArgb("#ff7a2f");

        // Show photo and save buttons
        PickPhotoButton.IsVisible = true;
        SaveButton.IsVisible = true;

        // Enable all input fields
        FirstNameEntry.IsEnabled = true;
        MiddleNameEntry.IsEnabled = true;
        LastNameEntry.IsEnabled = true;
        NationalityEntry.IsEnabled = true;
        CitizenshipEntry.IsEnabled = true;
        BirthDatePicker.IsEnabled = true;
        GenderPicker.IsEnabled = true;
        PlaceOfBirthEntry.IsEnabled = true;

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

        // Change button back to Edit
        EditButton.Text = "✏️ EDIT";
        EditButton.BackgroundColor = Color.FromArgb("#19b5ea");

        // Hide photo and save buttons
        PickPhotoButton.IsVisible = false;
        SaveButton.IsVisible = false;

        // Disable all input fields
        FirstNameEntry.IsEnabled = false;
        MiddleNameEntry.IsEnabled = false;
        LastNameEntry.IsEnabled = false;
        NationalityEntry.IsEnabled = false;
        CitizenshipEntry.IsEnabled = false;
        BirthDatePicker.IsEnabled = false;
        GenderPicker.IsEnabled = false;
        PlaceOfBirthEntry.IsEnabled = false;

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
