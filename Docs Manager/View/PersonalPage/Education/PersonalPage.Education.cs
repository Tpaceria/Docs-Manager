using Microsoft.Maui.Controls.Shapes;

namespace Docs_Manager.View;

public partial class PersonalPage
{
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

                        Margin =
                            new Thickness(0, 0, 0, 10)
                    };

                var layout =
                    new VerticalStackLayout
                    {
                        Spacing = 4
                    };

                // Qualification
                layout.Add(
                    new Label
                    {
                        Text = $" {education.Qualification}",

                        FontSize = 17,

                        FontAttributes =
                            FontAttributes.Bold,

                        TextColor =
                            Colors.White
                    });

                // Institution
                layout.Add(
                    new Label
                    {
                        Text =
                            education.Institution,

                        FontSize = 13,

                        TextColor =
                            Color.FromArgb("#9bb4d1")
                    });

                // Country • Year
                layout.Add(
                    new Label
                    {
                        Text =
                            $"{education.Country} • {education.GraduationDate:yyyy}",

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
}