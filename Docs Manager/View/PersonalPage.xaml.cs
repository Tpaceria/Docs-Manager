using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class PersonalPage : ContentPage
{
    private DatabaseService? _database;
    private string? _photoPath;

    public PersonalPage()
    {
        InitializeComponent();
    }

    private DatabaseService GetDatabase()
    {
        _database ??= ServiceHelper.GetService<DatabaseService>();
        return _database;
    }
    public async Task LoadProfileAsync()
    {
        try
        {
            var profile = await GetDatabase().GetUserProfileAsync();

            if (profile == null)
                return;

            FirstNameEntry.Text = profile.FirstName;
            LastNameEntry.Text = profile.LastName;
            EmailEntry.Text = profile.Email;
            PhoneEntry.Text = profile.Phone;

            BirthDatePicker.Date =
                profile.BirthDate == DateTime.MinValue
                    ? DateTime.Today
                    : profile.BirthDate;

            PositionEntry.Text = profile.Position;
            CitizenshipEntry.Text = profile.Citizenship;
            ResidenceEntry.Text = profile.Residence;
            ResidenceAirportEntry.Text = profile.ResidenceAirport;

            DesiredWageEntry.Text =
                profile.DesiredWage.ToString();

            HeightEntry.Text =
                profile.Height.ToString();

            WeightEntry.Text =
                profile.Weight.ToString();

            ShoeSizeEntry.Text =
                profile.ShoeSize.ToString();

            OverallSizeEntry.Text =
                profile.OverallSize.ToString();

            HairColorEntry.Text = profile.HairColor;
            EyeColorEntry.Text = profile.EyeColor;

            QualificationEntry.Text =
                profile.QualificationDegree;

            EducationInstitutionEntry.Text =
                profile.EducationInstitution;

            GraduationDatePicker.Date =
                profile.GraduationDate == DateTime.MinValue
                    ? DateTime.Today
                    : profile.GraduationDate;

            if (!string.IsNullOrEmpty(profile.PhotoPath)
                && File.Exists(profile.PhotoPath))
            {
                _photoPath = profile.PhotoPath;

                PhotoImage.Source =
                    new FileImageSource
                    {
                        File = _photoPath
                    };
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "LOAD ERROR",
                ex.ToString(),
                "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadProfileAsync();
    }    // =========================================
    // ВЫБОР ФОТО
    // =========================================

    private async void OnPickPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(
                new PickOptions
                {
                    PickerTitle = "Выберите фото",
                    FileTypes = FilePickerFileType.Images
                });

            if (result != null)
            {
                _photoPath = result.FullPath;

                PhotoImage.Source =
                    new FileImageSource
                    {
                        File = _photoPath
                    };

                PickPhotoButton.Text = "✅ Фото выбрано";

                PickPhotoButton.BackgroundColor =
                    Color.FromArgb("#28A745");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ошибка",
                $"Не удалось выбрать фото:\n{ex.Message}",
                "OK");
        }
    }

    // =========================================
    // СОХРАНЕНИЕ
    // =========================================

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await DisplayAlert("TEST", "BUTTON WORKS", "OK");

        try
        {
            var db = GetDatabase();

            // ===== ПОЛУЧАЕМ ПРОФИЛЬ =====

            var profile = await db.GetUserProfileAsync();

            // ===== ЕСЛИ НЕТ — СОЗДАЁМ =====

            if (profile == null)
            {
                profile = new UserProfile
                {
                    Id = 1
                };
            }

            // =====================================
            // ОСНОВНАЯ ИНФОРМАЦИЯ
            // =====================================

            profile.FirstName =
                FirstNameEntry.Text ?? "";

            profile.LastName =
                LastNameEntry.Text ?? "";

            profile.Email =
                EmailEntry.Text ?? "";

            profile.Phone =
                PhoneEntry.Text ?? "";

            profile.Age =
                CalculateAge(profile.BirthDate);
            // =====================================
            // ПРОФЕССИОНАЛЬНО
            // =====================================

            profile.Position =
                PositionEntry.Text ?? "";

            profile.Citizenship =
                CitizenshipEntry.Text ?? "";

            profile.Residence =
                ResidenceEntry.Text ?? "";

            profile.ResidenceAirport =
                ResidenceAirportEntry.Text ?? "";

            decimal.TryParse(
                DesiredWageEntry.Text,
                out decimal wage);

            profile.DesiredWage = wage;

            // =====================================
            // ЛИЧНОЕ
            // =====================================

            int.TryParse(
                HeightEntry.Text,
                out int height);

            profile.Height = height;

            int.TryParse(
                WeightEntry.Text,
                out int weight);

            profile.Weight = weight;

            int.TryParse(
                ShoeSizeEntry.Text,
                out int shoeSize);

            profile.ShoeSize = shoeSize;

            int.TryParse(
                OverallSizeEntry.Text,
                out int overallSize);

            profile.OverallSize = overallSize;

            profile.HairColor =
                HairColorEntry.Text ?? "";

            profile.EyeColor =
                EyeColorEntry.Text ?? "";

            // =====================================
            // ОБРАЗОВАНИЕ
            // =====================================

            profile.QualificationDegree =
                QualificationEntry.Text ?? "";

            profile.EducationInstitution =
                EducationInstitutionEntry.Text ?? "";

            profile.GraduationDate = GraduationDatePicker.Date.Value;
            // =====================================
            // ФОТО
            // =====================================

            profile.PhotoPath = _photoPath;

            // =====================================
            // ОБНОВЛЕНИЕ
            // =====================================

            profile.UpdatedDate = DateTime.Now;

            // =====================================
            // СОХРАНЕНИЕ В БАЗУ
            // =====================================

            await db.SaveUserProfileAsync(profile);

            await DisplayAlert(
                "Success",
                "Profile saved successfully!",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.ToString(),
                "OK");
        }
    }

    // =========================================
    // ОТМЕНА
    // =========================================

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // =========================================
    // ВОЗРАСТ
    // =========================================

    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;

        int age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
            age--;

        return age;
    }
}