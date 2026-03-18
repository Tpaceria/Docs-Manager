using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class PersonalPage : ContentPage
{
    private readonly DatabaseService _database = null!;
    private string? _photoPath;

    public PersonalPage(DatabaseService database)
    {
        InitializeComponent();
        _database = database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var profile = await _database.GetUserProfileAsync();

        if (profile != null)
        {
            // Основная информация
            FirstNameEntry.Text = profile.FirstName;
            LastNameEntry.Text = profile.LastName;
            EmailEntry.Text = profile.Email;
            PhoneEntry.Text = profile.Phone;
            BirthDatePicker.Date = profile.BirthDate != DateTime.MinValue
                ? profile.BirthDate
                : DateTime.Today;

            // Профессиональная информация
            PositionEntry.Text = profile.Position;
            CitizenshipEntry.Text = profile.Citizenship;
            ResidenceEntry.Text = profile.Residence;
            ResidenceAirportEntry.Text = profile.ResidenceAirport;
            DesiredWageEntry.Text = profile.DesiredWage > 0
                ? profile.DesiredWage.ToString()
                : "";

            // Личная информация
            HeightEntry.Text = profile.Height > 0 ? profile.Height.ToString() : "";
            WeightEntry.Text = profile.Weight > 0 ? profile.Weight.ToString() : "";
            ShoeSizeEntry.Text = profile.ShoeSize > 0 ? profile.ShoeSize.ToString() : "";
            OverallSizeEntry.Text = profile.OverallSize > 0 ? profile.OverallSize.ToString() : "";
            HairColorEntry.Text = profile.HairColor;
            EyeColorEntry.Text = profile.EyeColor;

            // Образование
            QualificationEntry.Text = profile.QualificationDegree;
            EducationInstitutionEntry.Text = profile.EducationInstitution;
            GraduationDatePicker.Date = profile.GraduationDate != DateTime.MinValue
                ? profile.GraduationDate
                : DateTime.Today;

            // Фото
            if (!string.IsNullOrEmpty(profile.PhotoPath) && File.Exists(profile.PhotoPath))
            {
                _photoPath = profile.PhotoPath;
                PhotoImage.Source = new FileImageSource { File = _photoPath };
            }
        }
    }

    private async void OnPickPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Выбери фото",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                _photoPath = result.FullPath;
                PhotoImage.Source = new FileImageSource { File = _photoPath };
                PickPhotoButton.Text = "✅ Фото выбрано";
                PickPhotoButton.BackgroundColor = Color.FromArgb("#28A745");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось выбрать фото: {ex.Message}", "OK");
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        try
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
            {
                await DisplayAlert("Ошибка", "Введи имя", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(LastNameEntry.Text))
            {
                await DisplayAlert("Ошибка", "Введи фамилию", "OK");
                return;
            }

            // Парси числовые поля
            int.TryParse(HeightEntry.Text, out int height);
            int.TryParse(WeightEntry.Text, out int weight);
            int.TryParse(ShoeSizeEntry.Text, out int shoeSize);
            int.TryParse(OverallSizeEntry.Text, out int overallSize);
            decimal.TryParse(DesiredWageEntry.Text, out decimal wage);

            var profile = new UserProfile
            {
                Id = 1,
                FirstName = FirstNameEntry.Text,
                LastName = LastNameEntry.Text,
                Email = EmailEntry.Text,
                Phone = PhoneEntry.Text,
                BirthDate = BirthDatePicker.Date ?? DateTime.Today,
                Age = CalculateAge(BirthDatePicker.Date ?? DateTime.Today),

                // Профессиональная информация
                Position = PositionEntry.Text,
                Citizenship = CitizenshipEntry.Text,
                Residence = ResidenceEntry.Text,
                ResidenceAirport = ResidenceAirportEntry.Text,
                DesiredWage = wage,

                // Личная информация
                Height = height,
                Weight = weight,
                ShoeSize = shoeSize,
                OverallSize = overallSize,
                HairColor = HairColorEntry.Text,
                EyeColor = EyeColorEntry.Text,

                // Образование
                QualificationDegree = QualificationEntry.Text,
                EducationInstitution = EducationInstitutionEntry.Text,
                GraduationDate = GraduationDatePicker.Date ?? DateTime.Today,

                // Фото
                PhotoPath = _photoPath,

                UpdatedDate = DateTime.Now
            };

            await _database.SaveUserProfileAsync(profile);
            await DisplayAlert("Успех", "Данные профиля сохранены!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Ошибка при сохранении: {ex.Message}", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
}