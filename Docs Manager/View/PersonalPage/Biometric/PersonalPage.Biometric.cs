namespace Docs_Manager.View;

public partial class PersonalPage
{
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
}