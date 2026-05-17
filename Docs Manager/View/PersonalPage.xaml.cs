using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class PersonalPage : ContentView
{
    private DatabaseService? _database;

    private string? _photoPath;

    public PersonalPage()
    {
        InitializeComponent();

        BirthDatePicker.Date = DateTime.Today;

        _ = LoadProfileAsync();
    }

    private DatabaseService GetDatabase()
    {
        _database ??=
            ServiceHelper.GetService<DatabaseService>();

        return _database!;
    }

    private async Task LoadProfileAsync()
    {
        try
        {
            var profile =
                await GetDatabase().GetUserProfileAsync();

            if (profile == null)
                return;

            FirstNameEntry.Text = profile.FirstName;

            LastNameEntry.Text = profile.LastName;

            PrimaryEmailEntry.Text = profile.Email;

            PrimaryPhoneEntry.Text = profile.Phone;

            BirthDatePicker.Date = profile.BirthDate;

            GenderPicker.SelectedItem = profile.Gender;

            CitizenshipEntry.Text = profile.Citizenship;

            ResidenceEntry.Text = profile.Residence;

            ResidenceAirportEntry.Text =
                profile.ResidenceAirport;

            if (!string.IsNullOrEmpty(profile.PhotoPath)
                && File.Exists(profile.PhotoPath))
            {
                _photoPath = profile.PhotoPath;

                PhotoImage.Source =
                    ImageSource.FromFile(_photoPath);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private async void OnPickPhotoClicked(
        object sender,
        EventArgs e)
    {
        try
        {
            var result =
                await FilePicker.Default.PickAsync(
                    new PickOptions
                    {
                        PickerTitle = "Select Photo",
                        FileTypes = FilePickerFileType.Images
                    });

            if (result != null)
            {
                _photoPath = result.FullPath;

                PhotoImage.Source =
                    ImageSource.FromFile(_photoPath);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
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
            var db = GetDatabase();

            var profile =
                await db.GetUserProfileAsync();

            if (profile == null)
            {
                profile = new UserProfile
                {
                    Id = 1
                };
            }

            profile.FirstName =
                FirstNameEntry.Text ?? "";

            profile.LastName =
                LastNameEntry.Text ?? "";

            profile.Email =
                PrimaryEmailEntry.Text ?? "";

            profile.Phone =
                PrimaryPhoneEntry.Text ?? "";

            profile.BirthDate = Convert.ToDateTime(BirthDatePicker.Date);

            profile.Gender = GenderPicker.SelectedItem?.ToString() ?? "";

            profile.Citizenship =
                CitizenshipEntry.Text ?? "";

            profile.Residence =
                ResidenceEntry.Text ?? "";

            profile.ResidenceAirport =
                ResidenceAirportEntry.Text ?? "";

            profile.PhotoPath =
                _photoPath;

            profile.UpdatedDate =
                DateTime.Now;

            await db.SaveUserProfileAsync(profile);

            await Application.Current.MainPage.DisplayAlert(
                "Saved",
                "Profile saved successfully",
                "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                ex.ToString(),
                "OK");
        }
    }
}