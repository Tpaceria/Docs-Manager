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

        Loaded += PersonalPage_Loaded;
    }

    // =====================================
    // PAGE LOADED
    // =====================================

    private async void PersonalPage_Loaded(
        object? sender,
        EventArgs e)
    {
        await LoadContactsAsync();
    }

    // =====================================
    // DATABASE
    // =====================================

    private DatabaseService GetDatabase()
    {
        _database ??=
            ServiceHelper.GetService<DatabaseService>();

        return _database!;
    }

    // =====================================
    // CONTACTS
    // =====================================

    private async void OnEditContactsClicked(
        object sender,
        EventArgs e)
    {
        var page = new AddContactPage();

        page.Disappearing += async (s, e2) =>
        {
            await LoadContactsAsync();
        };

        await Navigation.PushModalAsync(page);
    }

    private async Task LoadContactsAsync()
    {
        try
        {
            PhoneListContainer.Children.Clear();

            EmailListContainer.Children.Clear();

            var contacts =
                await GetDatabase().GetContactsAsync();

            foreach (var contact in contacts)
            {
                // PHONE

                if (contact.ContactType == "phone")
                {
                    string messengers = "";

                    if (contact.WhatsApp)
                        messengers += "WhatsApp • ";

                    if (contact.Telegram)
                        messengers += "Telegram • ";

                    if (contact.Viber)
                        messengers += "Viber • ";

                    messengers =
                        messengers.TrimEnd(' ', '•');

                    var frame = new Frame
                    {
                        BackgroundColor =
                            Color.FromArgb("#1a2238"),

                        BorderColor =
                            Color.FromArgb("#2d4d73"),

                        CornerRadius = 10,

                        Padding = 10,

                        HasShadow = false,

                        Content = new VerticalStackLayout
                        {
                            Spacing = 4,

                            Children =
                            {
                                new Label
                                {
                                    Text = "Phone",
                                    FontSize = 11,
                                    TextColor =
                                        Color.FromArgb("#7a8999")
                                },

                                new Label
                                {
                                    Text = contact.Value,
                                    FontSize = 22,
                                    FontAttributes =
                                        FontAttributes.Bold,
                                    TextColor = Colors.White
                                },

                                new Label
                                {
                                    Text = messengers,
                                    FontSize = 12,
                                    TextColor =
                                        Color.FromArgb("#00d4ff")
                                }
                            }
                        }
                    };

                    PhoneListContainer.Children.Add(frame);
                }

                // EMAIL

                if (contact.ContactType == "email")
                {
                    var frame = new Frame
                    {
                        BackgroundColor =
                            Color.FromArgb("#1a2238"),

                        BorderColor =
                            Color.FromArgb("#2d4d73"),

                        CornerRadius = 10,

                        Padding = 10,

                        HasShadow = false,

                        Content = new VerticalStackLayout
                        {
                            Spacing = 4,

                            Children =
                            {
                                new Label
                                {
                                    Text = "Email",
                                    FontSize = 11,
                                    TextColor =
                                        Color.FromArgb("#7a8999")
                                },

                                new Label
                                {
                                    Text = contact.Value,
                                    FontSize = 22,
                                    FontAttributes =
                                        FontAttributes.Bold,
                                    TextColor = Colors.White
                                }
                            }
                        }
                    };

                    EmailListContainer.Children.Add(frame);
                }
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

    // =====================================
    // EDUCATION
    // =====================================

    private async void OnAddEducationClicked(
        object sender,
        EventArgs e)
    {
        await Application.Current.MainPage.DisplayAlert(
            "Education",
            "Add Education popup coming soon",
            "OK");
    }

    // =====================================
    // LOAD PROFILE
    // =====================================

    private async Task LoadProfileAsync()
    {
        try
        {
            var profile =
                await GetDatabase().GetUserProfileAsync();

            if (profile == null)
                return;

            // BASIC

            FirstNameEntry.Text =
                profile.FirstName;

            LastNameEntry.Text =
                profile.LastName;

            BirthDatePicker.Date =
                profile.BirthDate;

            GenderPicker.SelectedItem =
                profile.Gender;

            CitizenshipEntry.Text =
                profile.Citizenship;

            // ADDRESS

            ResidenceEntry.Text =
                profile.Residence;

            ResidenceAirportEntry.Text =
                profile.ResidenceAirport;

            // PHOTO

            if (!string.IsNullOrEmpty(profile.PhotoPath)
                && File.Exists(profile.PhotoPath))
            {
                _photoPath =
                    profile.PhotoPath;

                PhotoImage.Source =
                    ImageSource.FromFile(_photoPath);
            }

            // PERSONAL

            HeightEntry.Text =
                profile.Height == 0
                    ? ""
                    : profile.Height.ToString();

            WeightEntry.Text =
                profile.Weight == 0
                    ? ""
                    : profile.Weight.ToString();

            ShoeSizeEntry.Text =
                profile.ShoeSize == 0
                    ? ""
                    : profile.ShoeSize.ToString();

            OverallSizeEntry.Text =
                profile.OverallSize == 0
                    ? ""
                    : profile.OverallSize.ToString();

            // APPEARANCE

            HairColorEntry.Text =
                profile.HairColor;

            EyeColorEntry.Text =
                profile.EyeColor;
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    // =====================================
    // PHOTO
    // =====================================

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
                        FileTypes =
                            FilePickerFileType.Images
                    });

            if (result != null)
            {
                _photoPath =
                    result.FullPath;

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

    // =====================================
    // SAVE PROFILE
    // =====================================

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

            // BASIC

            profile.FirstName =
                FirstNameEntry.Text ?? "";

            profile.LastName =
                LastNameEntry.Text ?? "";

            profile.BirthDate =
                BirthDatePicker.Date ?? DateTime.Today;

            profile.Gender =
                GenderPicker.SelectedItem?.ToString() ?? "";

            profile.Citizenship =
                CitizenshipEntry.Text ?? "";

            // ADDRESS

            profile.Residence =
                ResidenceEntry.Text ?? "";

            profile.ResidenceAirport =
                ResidenceAirportEntry.Text ?? "";

            // PERSONAL

            profile.Height =
                int.TryParse(
                    HeightEntry.Text,
                    out var height)
                        ? height
                        : 0;

            profile.Weight =
                int.TryParse(
                    WeightEntry.Text,
                    out var weight)
                        ? weight
                        : 0;

            profile.ShoeSize =
                int.TryParse(
                    ShoeSizeEntry.Text,
                    out var shoe)
                        ? shoe
                        : 0;

            profile.OverallSize =
                int.TryParse(
                    OverallSizeEntry.Text,
                    out var overall)
                        ? overall
                        : 0;

            // APPEARANCE

            profile.HairColor =
                HairColorEntry.Text ?? "";

            profile.EyeColor =
                EyeColorEntry.Text ?? "";

            // PHOTO

            profile.PhotoPath =
                _photoPath;

            profile.UpdatedDate =
                DateTime.Now;

            // SAVE

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
    private async void OnEditEducationClicked(
    object sender,
    EventArgs e)
    {
        var page = new AddEducationPage();

        page.Disappearing += async (s, e2) =>
        {
            await LoadEducationAsync();
        };

        await Navigation.PushModalAsync(page);
    }

    private async Task LoadEducationAsync()
    {
        try
        {
            EducationContainer.Children.Clear();

            var educationList =
                await GetDatabase().GetEducationAsync();

            foreach (var education in educationList)
            {
                var frame = new Frame
                {
                    BackgroundColor =
                        Color.FromArgb("#1a2238"),

                    BorderColor =
                        Color.FromArgb("#2d4d73"),

                    CornerRadius = 10,

                    Padding = 10,

                    HasShadow = false,

                    Content = new VerticalStackLayout
                    {
                        Spacing = 4,

                        Children =
                    {
                        new Label
                        {
                            Text = education.Qualification,
                            FontSize = 18,
                            FontAttributes =
                                FontAttributes.Bold,
                            TextColor = Colors.White
                        },

                        new Label
                        {
                            Text = education.Institution,
                            FontSize = 13,
                            TextColor =
                                Color.FromArgb("#00d4ff")
                        },

                        new Label
                        {
                            Text =
                                education.GraduationDate
                                .ToString("dd.MM.yyyy"),

                            FontSize = 12,

                            TextColor =
                                Color.FromArgb("#7a8999")
                        }
                    }
                    }
                };

                EducationContainer.Children.Add(frame);
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
}
