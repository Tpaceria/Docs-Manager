namespace Docs_Manager.View;

public partial class PersonalPage : ContentView
{
    public PersonalPage()
    {
        InitializeComponent();

        BirthDatePicker.Date = DateTime.Today;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.DisplayAlert(
            "Saved",
            "Profile saved successfully",
            "OK");
    }
}