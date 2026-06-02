using Docs_Manager.Data;
using Docs_Manager.Models;
using Microsoft.Maui.Controls.Shapes;

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
        var page =
            new AddContactPage();

        page.Disappearing += async (s, e2) =>
        {
            await LoadContactsPreview();
        };

        await Navigation.PushModalAsync(page);
    }

    private async Task LoadContactsPreview()
    {
        try
        {
            var contacts =
                await GetDatabase().GetContactsAsync();

            ContactsPreviewContainer.Clear();

            foreach (var contact in contacts)
            {
                var border =
                    new Border
                    {
                        BackgroundColor =
                            Color.FromArgb("#1a2238"),

                        Stroke =
                            Color.FromArgb("#224b75"),

                        StrokeShape =
                            new RoundRectangle
                            {
                                CornerRadius = 10
                            },

                        Padding = 10,

                        Margin =
                            new Thickness(0, 0, 0, 10)
                    };

                var layout =
                    new VerticalStackLayout
                    {
                        Spacing = 4
                    };

                // PHONE

                if (contact.ContactType == "phone")
                {
                    layout.Add(
                        new Label
                        {
                            Text = "Phone",
                            FontSize = 12,
                            TextColor =
                                Color.FromArgb("#8ea9c7")
                        });

                    layout.Add(
                        new Label
                        {
                            Text = contact.Value,
                            FontSize = 18,
                            FontAttributes =
                                FontAttributes.Bold,
                            TextColor = Colors.White
                        });

                    var messengers =
                        new List<string>();

                    if (contact.WhatsApp)
                        messengers.Add("WhatsApp");

                    if (contact.Telegram)
                        messengers.Add("Telegram");

                    if (contact.Viber)
                        messengers.Add("Viber");

                    if (messengers.Count > 0)
                    {
                        layout.Add(
                            new Label
                            {
                                Text =
                                    string.Join(
                                        " • ",
                                        messengers),

                                FontSize = 12,

                                TextColor =
                                    Color.FromArgb("#19b5ea")
                            });
                    }
                }

                // EMAIL

                if (contact.ContactType == "email")
                {
                    layout.Add(
                        new Label
                        {
                            Text = "Email",

                            FontSize = 12,

                            Margin =
                                new Thickness(0, 8, 0, 0),

                            TextColor =
                                Color.FromArgb("#8ea9c7")
                        });

                    layout.Add(
                        new Label
                        {
                            Text = contact.Value,

                            FontSize = 16,

                            FontAttributes =
                                FontAttributes.Bold,

                            TextColor = Colors.White
                        });
                }

                border.Content = layout;

                ContactsPreviewContainer.Add(border);
            }
        }
        catch
        {

        }
    }
    // =====================================
    // EDUCATION
    // =====================================

    private async void OnEditEducationClicked(
        object sender,
        EventArgs e)
    {
        var page =
            new AddEducationPage();

        page.Disappearing += async (s, e2) =>
        {
            await LoadEducationPreview();
        };

        await Navigation.PushModalAsync(page);
    }

    private async Task LoadEducationPreview()
    {
        try
        {
            var educationList =
                await GetDatabase().GetEducationAsync();

            EducationPreviewContainer.Clear();

            foreach (var education in educationList)
            {
                var border =
                    new Border
                    {
                        BackgroundColor =
                            Color.FromArgb("#1a2238"),

                        Stroke =
                            Color.FromArgb("#224b75"),

                        StrokeShape =
                            new RoundRectangle
                            {
                                CornerRadius = 10
                            },

                        Padding = 10,

                        Margin = new Thickness(0, 0, 0, 10)
                    };

                var layout =
                    new VerticalStackLayout
                    {
                        Spacing = 3
                    };

                layout.Add(
                    new Label
                    {
                        Text =
                            education.Qualification,

                        FontSize = 17,

                        FontAttributes =
                            FontAttributes.Bold,

                        TextColor =
                            Colors.White
                    });

                layout.Add(
                    new Label
                    {
                        Text =
                            education.Institution,

                        FontSize = 13,

                        TextColor =
                            Color.FromArgb("#9bb4d1")
                    });

                layout.Add(
                    new Label
                    {
                        Text =
                            education.GraduationDate
                                .Year
                                .ToString(),

                        FontSize = 12,

                        TextColor =
                            Color.FromArgb("#19b5ea")
                    });

                border.Content = layout;

                EducationPreviewContainer.Add(border);
            }
        }
        catch
        {

        }
    }

    // =====================================
    // LOAD PROFILE
    // =====================================

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

            LastNameEntry.Text =
                profile.LastName;

            BirthDatePicker.Date =
                profile.BirthDate;

            GenderPicker.SelectedItem =
                profile.Gender;

            CitizenshipEntry.Text =
                profile.Citizenship;

            ResidenceEntry.Text =
                profile.Residence;

            ResidenceAirportEntry.Text =
                profile.ResidenceAirport;

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

            HeightEntry.Text =
                profile.Height.ToString();

            WeightEntry.Text =
                profile.Weight.ToString();

            ShoeSizeEntry.Text =
                profile.ShoeSize.ToString();

            OverallSizeEntry.Text =
                profile.OverallSize.ToString();

            HairColorEntry.Text =
                profile.HairColor;


            EyeColorEntry.Text =
                profile.EyeColor;

            KinNameEntry.Text =
    profile.KinName;

            KinRelationEntry.Text =
                profile.KinRelation;

            KinPhoneEntry.Text =
                profile.KinPhone;

            KinEmailEntry.Text =
                profile.KinEmail;

            KinAddressEditor.Text =
                profile.KinAddress;
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

    // =====================================
    // SAVE PROFILE
    // =====================================

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

            profile.LastName =
                LastNameEntry.Text ?? "";

            profile.BirthDate =
                BirthDatePicker.Date ?? DateTime.Today;
            profile.Gender =
                GenderPicker.SelectedItem?
                    .ToString() ?? "";

            profile.Citizenship =
                CitizenshipEntry.Text ?? "";

            profile.Residence =
                ResidenceEntry.Text ?? "";

            profile.ResidenceAirport =
                ResidenceAirportEntry.Text ?? "";

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

            profile.HairColor =
                HairColorEntry.Text ?? "";

            profile.EyeColor =
                EyeColorEntry.Text ?? "";

            profile.KinName =
    KinNameEntry.Text ?? "";

            profile.KinRelation =
                KinRelationEntry.Text ?? "";

            profile.KinPhone =
                KinPhoneEntry.Text ?? "";

            profile.KinEmail =
                KinEmailEntry.Text ?? "";

            profile.KinAddress =
                KinAddressEditor.Text ?? "";

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


    private async void PersonalPage_Loaded(
    object? sender,
    EventArgs e)
    {
        await LoadContactsPreview();

        await LoadEducationPreview();
    }
}
