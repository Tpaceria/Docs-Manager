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

    private async void OnEditVisaClicked(
    object sender,
    EventArgs e)
    {
        var page =
            new AddVisaPage();

        page.Disappearing += async (s, e2) =>
        {
            await LoadVisaPreview();
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

                    Padding = 10
                };

            var layout =
                new VerticalStackLayout
                {
                    Spacing = 4
                };

            foreach (var contact in contacts)
            {
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

                    layout.Add(
                        new BoxView
                        {
                            HeightRequest = 1,
                            Margin = new Thickness(0, 6),
                            Color = Color.FromArgb("#224b75")
                        });
                }

                // EMAIL

                if (contact.ContactType == "email")
                {
                    layout.Add(
                        new Label
                        {
                            Text = "Email",
                            FontSize = 12,
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
                    layout.Add(
    new BoxView
    {
        HeightRequest = 1,
        Margin = new Thickness(0, 6),
        Color = Color.FromArgb("#224b75")
    });
                }

                // RESIDENCE

                if (contact.ContactType == "residence")
                {
                    layout.Add(
                        new Label
                        {
                            Text = "Residence",
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
                    layout.Add(
    new BoxView
    {
        HeightRequest = 1,
        Margin = new Thickness(0, 6),
        Color = Color.FromArgb("#224b75")
    });
                }

                // AIRPORT

                if (contact.ContactType == "airport")
                {
                    layout.Add(
                        new Label
                        {
                            Text = "Nearest Airport",
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
            }

            border.Content = layout;

            ContactsPreviewContainer.Add(border);
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

    private async void OnEditSkillsClicked(
        object sender,
        EventArgs e)
    {
        var page =
            new AddSkillsPage();

        page.Disappearing += async (s, e2) =>
        {
            await LoadSkillsPreview();
        };

        await Navigation.PushModalAsync(page);
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

    private async Task LoadVisaPreview()
    {
        try
        {
            var visas =
                await GetDatabase()
                    .GetVisasAsync();

            VisaPreviewContainer.Clear();

            foreach (var visa in visas)
            {
                var grid =
                    new Grid
                    {
                        ColumnDefinitions =
                        {
                        new ColumnDefinition
                        {
                            Width = GridLength.Star
                        },
                        new ColumnDefinition
                        {
                            Width = GridLength.Star
                        },
                        new ColumnDefinition
                        {
                            Width = GridLength.Auto
                        }
                        }
                    };

                grid.Add(
                    new Label
                    {
                        Text = visa.Type,
                        TextColor = Colors.White
                    }, 0, 0);

                grid.Add(
                    new Label
                    {
                        Text = visa.Country,
                        TextColor = Colors.White
                    }, 1, 0);

                grid.Add(
                    new Label
                    {
                        Text =
                            visa.ExpiryDate
                                .Year
                                .ToString(),

                        TextColor =
                            Color.FromArgb("#19b5ea")
                    }, 2, 0);

                VisaPreviewContainer.Add(grid);
            }
        }
        catch
        {
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
        await LoadProfileAsync();

        await LoadContactsPreview();

        await LoadEducationPreview();

        await LoadVisaPreview();

        await LoadSkillsPreview();

        await LoadBiometricPreview();

        await LoadNextOfKinPreview();
    }

    private async Task LoadSkillsPreview()
    {
        try
        {
            var skills =
                await GetDatabase()
                    .GetSkillsAsync();

            SkillsPreviewContainer.Clear();

            var skill =
                skills.FirstOrDefault();

            if (skill == null)
                return;

            SkillsPreviewContainer.Add(
                new Label
                {
                    Text =
                        $"English Level: {skill.EnglishLevel}",
                    TextColor = Colors.White
                });

            SkillsPreviewContainer.Add(
                new Label
                {
                    Text =
                        $"Marlins Test: {skill.MarlinsScore}%",
                    TextColor = Colors.White
                });

            SkillsPreviewContainer.Add(
                new Label
                {
                    Text =
                        $"CES Score: {skill.CesScore}%",
                    TextColor = Colors.White
                });

            SkillsPreviewContainer.Add(
                new Label
                {
                    Text =
                        $"Languages: {skill.AdditionalLanguages}",
                    TextColor = Colors.White
                });
        }
        catch
        {
        }
    }

    private async void OnEditBiometricClicked(
        object sender,
        EventArgs e)
    {
        var page =
            new AddBiometricPage();

        page.Disappearing += async (s, e2) =>
        {
            await LoadBiometricPreview();
        };

        await Navigation.PushModalAsync(page);
    }

    private async Task LoadBiometricPreview()
    {
        try
        {
            var biometric =
                (await GetDatabase()
                    .GetBiometricAsync())
                    .FirstOrDefault();

            BiometricPreviewContainer.Clear();

            if (biometric == null)
                return;

            var grid = new Grid
            {
                ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            },

                RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),

                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),

                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            },

                ColumnSpacing = 10,
                RowSpacing = 4
            };

            // HEIGHT / WEIGHT

            grid.Add(new Label
            {
                Text = "Height",
                FontSize = 12,
                TextColor = Color.FromArgb("#8ea9c7")
            }, 0, 0);

            grid.Add(new Label
            {
                Text = "Weight",
                FontSize = 12,
                TextColor = Color.FromArgb("#8ea9c7")
            }, 1, 0);

            grid.Add(new Label
            {
                Text = $"{biometric.Height} cm",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            }, 0, 1);

            grid.Add(new Label
            {
                Text = $"{biometric.Weight} kg",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            }, 1, 1);

            // SHOE / OVERALL

            grid.Add(new Label
            {
                Text = "Shoe Size",
                FontSize = 12,
                TextColor = Color.FromArgb("#8ea9c7")
            }, 0, 2);

            grid.Add(new Label
            {
                Text = "Overall Size",
                FontSize = 12,
                TextColor = Color.FromArgb("#8ea9c7")
            }, 1, 2);

            grid.Add(new Label
            {
                Text = biometric.ShoeSize.ToString(),
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            }, 0, 3);

            grid.Add(new Label
            {
                Text = biometric.OverallSize.ToString(),
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            }, 1, 3);

            // HAIR / EYES

            grid.Add(new Label
            {
                Text = "Hair Color",
                FontSize = 12,
                TextColor = Color.FromArgb("#8ea9c7")
            }, 0, 4);

            grid.Add(new Label
            {
                Text = "Eye Color",
                FontSize = 12,
                TextColor = Color.FromArgb("#8ea9c7")
            }, 1, 4);

            grid.Add(new Label
            {
                Text = biometric.HairColor,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            }, 0, 5);

            grid.Add(new Label
            {
                Text = biometric.EyeColor,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            }, 1, 5);

            BiometricPreviewContainer.Add(grid);
        }
        catch
        {
        }
    }
    private async void OnEditNextOfKinClicked(
    object sender,
    EventArgs e)
    {
        var page =
            new AddNextOfKinPage();

        page.Disappearing += async (s, e2) =>
        {
            await LoadNextOfKinPreview();
        };

        await Navigation.PushModalAsync(page);
    }
    private async Task LoadNextOfKinPreview()
    {
        try
        {
            var profile =
                await GetDatabase()
                    .GetUserProfileAsync();

            NextOfKinPreviewContainer.Clear();

            if (profile == null)
                return;

            AddPreviewItem(
                "Full Name",
                profile.KinName);

            AddPreviewItem(
                "Relation",
                profile.KinRelation);

            AddPreviewItem(
                "Phone",
                profile.KinPhone);

            AddPreviewItem(
                "Email",
                profile.KinEmail);

            AddPreviewItem(
                "Address",
                profile.KinAddress);
        }
        catch
        {
        }
    }

    private void AddPreviewItem(
        string title,
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        NextOfKinPreviewContainer.Add(
            new Label
            {
                Text = title,
                FontSize = 12,
                TextColor =
                    Color.FromArgb("#8ea9c7")
            });

        NextOfKinPreviewContainer.Add(
            new Label
            {
                Text = value,
                FontSize = 15,
                FontAttributes =
                    FontAttributes.Bold,
                TextColor = Colors.White
            });
    }
}
