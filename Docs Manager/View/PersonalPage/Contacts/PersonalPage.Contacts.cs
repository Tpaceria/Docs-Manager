using Microsoft.Maui.Controls.Shapes;

namespace Docs_Manager.View;

public partial class PersonalPage
{
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
}