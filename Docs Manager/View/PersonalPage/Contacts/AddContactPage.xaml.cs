using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddContactPage : ContentPage
{
    readonly DatabaseService _database;

    public AddContactPage()
    {
        InitializeComponent();

        _database =
            ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException();

        _ = LoadContactsAsync();
    }

    // =====================================
    // LOAD CONTACTS
    // =====================================

    private async Task LoadContactsAsync()
    {
        try
        {
            PhoneContainer.Children.Clear();

            EmailContainer.Children.Clear();

            var contacts =
                await _database.GetContactsAsync();

            foreach (var contact in contacts)
            {
                if (contact.ContactType == "phone")
                {
                    AddPhoneBlock(contact);
                }

                if (contact.ContactType == "email")
                {
                    AddEmailBlock(contact.Value ?? "");
                }
            }

            var residence =
    contacts.FirstOrDefault(
        x => x.ContactType == "residence");

            if (residence != null)
            {
                ResidenceEntry.Text = residence.Value;
            }

            var airport =
                contacts.FirstOrDefault(
                    x => x.ContactType == "airport");

            if (airport != null)
            {
                NearestAirportEntry.Text = airport.Value;
            }

            // ЕСЛИ НЕТ КОНТАКТОВ

            if (PhoneContainer.Children.Count == 0)
            {
                AddPhoneBlock(null);
            }

            if (EmailContainer.Children.Count == 0)
            {
                AddEmailBlock();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    // =====================================
    // PHONE BLOCK
    // =====================================

    private void AddPhoneBlock(
        ContactInfo? contact = null)
    {
        var deleteButton = new Button
        {
            Text = "🗑",
            WidthRequest = 34,
            HeightRequest = 34,
            CornerRadius = 17,
            BackgroundColor = Color.FromArgb("#ff4d6d"),
            TextColor = Colors.White,
            FontSize = 12
        };

        var headerGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        headerGrid.Add(new Label
        {
            Text = "Phone",
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold
        });

        headerGrid.Add(deleteButton, 1, 0);

        var phoneEntry = new Entry
        {
            Placeholder = "+47...",
            Text = contact?.Value ?? "",
            TextColor = Colors.White,
            PlaceholderColor = Color.FromArgb("#7a8999"),
            BackgroundColor = Color.FromArgb("#1a2238")
        };

        // CHECKBOXES

        var whatsapp = new CheckBox
        {
            IsChecked = contact?.WhatsApp ?? false
        };

        var telegram = new CheckBox
        {
            IsChecked = contact?.Telegram ?? false
        };

        var viber = new CheckBox
        {
            IsChecked = contact?.Viber ?? false
        };

        var messengerRow = new HorizontalStackLayout
        {
            Spacing = 20,
            Children =
            {
                whatsapp,

                new Label
                {
                    Text = "WhatsApp",
                    TextColor = Colors.White,
                    VerticalOptions = LayoutOptions.Center
                },

                telegram,

                new Label
                {
                    Text = "Telegram",
                    TextColor = Colors.White,
                    VerticalOptions = LayoutOptions.Center
                },

                viber,

                new Label
                {
                    Text = "Viber",
                    TextColor = Colors.White,
                    VerticalOptions = LayoutOptions.Center
                }
            }
        };

        var content = new VerticalStackLayout
        {
            Spacing = 10,
            Children =
            {
                headerGrid,
                phoneEntry,
                messengerRow
            }
        };

        var frame = new Frame
        {
            BackgroundColor = Color.FromArgb("#163454"),
            BorderColor = Color.FromArgb("#2d4d73"),
            CornerRadius = 10,
            Padding = 10,
            HasShadow = false,
            Content = content
        };

        deleteButton.Clicked += (s, e) =>
        {
            PhoneContainer.Children.Remove(frame);
        };

        PhoneContainer.Children.Add(frame);
    }

    // =====================================
    // EMAIL BLOCK
    // =====================================

    private void AddEmailBlock(
        string value = "")
    {
        var deleteButton = new Button
        {
            Text = "🗑",
            WidthRequest = 34,
            HeightRequest = 34,
            CornerRadius = 17,
            BackgroundColor = Color.FromArgb("#ff4d6d"),
            TextColor = Colors.White,
            FontSize = 12
        };

        var headerGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        headerGrid.Add(new Label
        {
            Text = "Email",
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold
        });

        headerGrid.Add(deleteButton, 1, 0);

        var emailEntry = new Entry
        {
            Placeholder = "example@email.com",
            Text = value,
            TextColor = Colors.White,
            PlaceholderColor = Color.FromArgb("#7a8999"),
            BackgroundColor = Color.FromArgb("#1a2238")
        };

        var layout = new VerticalStackLayout
        {
            Spacing = 10,
            Children =
            {
                headerGrid,
                emailEntry
            }
        };

        var frame = new Frame
        {
            BackgroundColor = Color.FromArgb("#163454"),
            BorderColor = Color.FromArgb("#2d4d73"),
            CornerRadius = 10,
            Padding = 10,
            HasShadow = false,
            Content = layout
        };

        deleteButton.Clicked += (s, e) =>
        {
            EmailContainer.Children.Remove(frame);
        };

        EmailContainer.Children.Add(frame);
    }

    // =====================================
    // ADD BUTTONS
    // =====================================

    private void OnAddPhoneClicked(
        object sender,
        EventArgs e)
    {
        AddPhoneBlock(null);
    }

    private void OnAddEmailClicked(
        object sender,
        EventArgs e)
    {
        AddEmailBlock();
    }

    // =====================================
    // CLOSE
    // =====================================

    private async void OnCloseClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    // =====================================
    // SAVE CONTACTS
    // =====================================

    private async void OnSaveClicked(
        object sender,
        EventArgs e)
    {
        try
        {
            // DELETE OLD CONTACTS

            var oldContacts =
                await _database.GetContactsAsync();

            foreach (var oldContact in oldContacts)
            {
                await _database.DeleteContactAsync(oldContact);
            }

            // SAVE PHONES

            foreach (var child in PhoneContainer.Children)
            {
                if (child is Frame frame
                    && frame.Content is VerticalStackLayout layout)
                {
                    var phoneEntry =
                        layout.Children
                            .OfType<Entry>()
                            .FirstOrDefault();

                    var messengerRow =
                        layout.Children
                            .OfType<HorizontalStackLayout>()
                            .FirstOrDefault();

                    if (phoneEntry == null)
                        continue;

                    if (string.IsNullOrWhiteSpace(phoneEntry.Text))
                        continue;

                    bool whatsapp = false;
                    bool telegram = false;
                    bool viber = false;

                    if (messengerRow != null)
                    {
                        var checkboxes =
                            messengerRow.Children
                                .OfType<CheckBox>()
                                .ToList();

                        if (checkboxes.Count >= 3)
                        {
                            whatsapp = checkboxes[0].IsChecked;
                            telegram = checkboxes[1].IsChecked;
                            viber = checkboxes[2].IsChecked;
                        }
                    }

                    var contact = new ContactInfo
                    {
                        ContactType = "phone",
                        Value = phoneEntry.Text,

                        WhatsApp = whatsapp,
                        Telegram = telegram,
                        Viber = viber
                    };

                    await _database.SaveContactAsync(contact);
                }
            }

            // SAVE EMAILS

            foreach (var child in EmailContainer.Children)
            {
                if (child is Frame frame
                    && frame.Content is VerticalStackLayout layout)
                {
                    var emailEntry =
                        layout.Children
                            .OfType<Entry>()
                            .FirstOrDefault();

                    if (emailEntry == null)
                        continue;

                    if (string.IsNullOrWhiteSpace(emailEntry.Text))
                        continue;

                    var contact = new ContactInfo
                    {
                        ContactType = "email",
                        Value = emailEntry.Text
                    };

                    await _database.SaveContactAsync(contact);
                }
            }

            // SAVE RESIDENCE

            if (!string.IsNullOrWhiteSpace(ResidenceEntry.Text))
            {
                await _database.SaveContactAsync(
                    new ContactInfo
                    {
                        ContactType = "residence",
                        Value = ResidenceEntry.Text
                    });
            }

            // SAVE AIRPORT

            if (!string.IsNullOrWhiteSpace(NearestAirportEntry.Text))
            {
                await _database.SaveContactAsync(
                    new ContactInfo
                    {
                        ContactType = "airport",
                        Value = NearestAirportEntry.Text
                    });
            }
        

            await DisplayAlert(
                "Saved",
                "Contacts saved successfully",
                "OK");

            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }

    }
}