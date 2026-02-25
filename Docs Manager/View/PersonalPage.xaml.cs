using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class PersonalPage : ContentPage
{
    private readonly DatabaseService _database = null!;

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
            FirstNameEntry.Text = profile.FirstName;
            LastNameEntry.Text = profile.LastName;
            EmailEntry.Text = profile.Email;
            BirthDatePicker.Date = profile.BirthDate;
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        var profile = new UserProfile
        {
            Id = 1,
            FirstName = FirstNameEntry.Text,
            LastName = LastNameEntry.Text,
            Email = EmailEntry.Text,
            BirthDate = BirthDatePicker.Date ?? DateTime.Today
        };

        await _database.SaveUserProfileAsync(profile);

        await DisplayAlertAsync("Сохранено", "Данные профиля сохранены", "OK");
    }
}