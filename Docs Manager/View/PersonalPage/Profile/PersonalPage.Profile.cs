using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class PersonalPage
{
    private async Task LoadProfileAsync()
    {
        try
        {
            var profile =
                await GetDatabase()
                    .GetUserProfileAsync();

            if (profile == null)
                return;

            FirstNameEntry.Text =
                profile.FirstName;

            MiddleNameEntry.Text =
                profile.MiddleName;

            LastNameEntry.Text =
                profile.LastName;

            BirthDatePicker.Date =
                profile.BirthDate;

            GenderPicker.SelectedItem =
                profile.Gender;

            CitizenshipEntry.Text =
                profile.Citizenship;

            if (!string.IsNullOrEmpty(
                profile.PhotoPath)
                && File.Exists(profile.PhotoPath))
            {
                _photoPath =
                    profile.PhotoPath;

                PhotoImage.Source =
                    ImageSource.FromFile(
                        _photoPath);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"ERROR: {ex}");
        }
    }

    private async void OnPickPhotoClicked(
        object sender,
        EventArgs e)
    {
        try
        {
            var result =
                await FilePicker.Default
                    .PickAsync(
                        new PickOptions
                        {
                            PickerTitle =
                                "Select Photo",

                            FileTypes =
                                FilePickerFileType.Images
                        });

            if (result != null)
            {
                _photoPath =
                    result.FullPath;

                PhotoImage.Source =
                    ImageSource.FromFile(
                        _photoPath);
            }
        }
        catch (Exception ex)
        {
            await Application.Current
                .MainPage
                .DisplayAlert(
                    "Error",
                    ex.Message,
                    "OK");
        }
    }

    private async void OnSaveClicked(
        object sender,
        EventArgs e)
    {
        try
        {
            var db =
                GetDatabase();

            var profile =
                await db.GetUserProfileAsync();

            if (profile == null)
            {
                profile =
                    new UserProfile
                    {
                        Id = 1
                    };
            }

            profile.FirstName =
                FirstNameEntry.Text ?? "";

            profile.MiddleName =
                MiddleNameEntry.Text ?? "";

            profile.LastName =
                LastNameEntry.Text ?? "";

            profile.BirthDate =
                BirthDatePicker.Date ?? DateTime.Today;

            profile.Gender =
                GenderPicker.SelectedItem?
                    .ToString() ?? "";

            profile.Citizenship =
                CitizenshipEntry.Text ?? "";

            profile.PhotoPath =
                _photoPath;

            profile.UpdatedDate =
                DateTime.Now;

            await db.SaveUserProfileAsync(
                profile);

            await Application.Current
                .MainPage
                .DisplayAlert(
                    "Saved",
                    "Profile saved successfully",
                    "OK");

            // Reload profile data
            await LoadProfileAsync();

            // Reload all preview sections
            await LoadContactsPreview();
            await LoadEducationPreview();
            await LoadCocPreview();
            await LoadCertificatesPreview();
            await LoadExperiencePreview();
            await LoadDocumentsPreview();
            await LoadSkillsPreview();
            await LoadBiometricPreview();
            await LoadNextOfKinPreview();

            // Exit edit mode after saving
            ExitEditMode();
        }
        catch (Exception ex)
        {
            await Application.Current
                .MainPage
                .DisplayAlert(
                    "Error",
                    ex.ToString(),
                    "OK");
        }
    }
}
