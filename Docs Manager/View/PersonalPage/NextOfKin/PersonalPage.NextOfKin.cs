
namespace Docs_Manager.View;

public partial class PersonalPage
{
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
