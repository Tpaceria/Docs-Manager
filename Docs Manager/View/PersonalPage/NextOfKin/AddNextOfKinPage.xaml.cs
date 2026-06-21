using Docs_Manager.Data;

namespace Docs_Manager.View;

public partial class AddNextOfKinPage : ContentPage
{
    readonly DatabaseService _database;

    public AddNextOfKinPage()
    {
        InitializeComponent();

        _database =
            ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException();

        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var profile =
            await _database.GetUserProfileAsync();

        if (profile == null)
            return;

        NameEntry.Text =
            profile.KinName;

        RelationEntry.Text =
            profile.KinRelation;

        PhoneEntry.Text =
            profile.KinPhone;

        EmailEntry.Text =
            profile.KinEmail;

        AddressEditor.Text =
            profile.KinAddress;
    }

    private async void OnSaveClicked(
        object sender,
        EventArgs e)
    {
        var profile =
            await _database.GetUserProfileAsync()
            ?? new Models.UserProfile();

        profile.KinName =
            NameEntry.Text ?? "";

        profile.KinRelation =
            RelationEntry.Text ?? "";

        profile.KinPhone =
            PhoneEntry.Text ?? "";

        profile.KinEmail =
            EmailEntry.Text ?? "";

        profile.KinAddress =
            AddressEditor.Text ?? "";

        await _database.SaveUserProfileAsync(
            profile);

        await Navigation.PopModalAsync();
    }

    private async void OnCloseClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}