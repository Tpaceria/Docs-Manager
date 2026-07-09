using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace Docs_Manager.Helper;

public class DocumentCardModel
{
    public string Title { get; set; } = "";
    public string Country { get; set; } = "";
    public string Number { get; set; } = "";
    public DateTime IssueDate { get; set; }
    public string Expiry { get; set; } = "";

    public Action? ViewAction { get; set; }
    public Action? EditAction { get; set; }
    public Action? DeleteAction { get; set; }
}

public static class DocumentCardBuilder
{
    public static Border CreateDocumentCard(DocumentCardModel model)
    {
        var border = new Border
        {
            BackgroundColor = Color.FromArgb("#102544"),
            Stroke = Color.FromArgb("#17365d"),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 18
            },
            Padding = 18
        };

        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            }
        };

        //==========================
        // TITLE
        //==========================

        grid.Add(new Label
        {
            Text = model.Title,
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White
        }, 0, 0);

        //==========================
        // INFO
        //==========================

        var info = new HorizontalStackLayout
        {
            Spacing = 35,
            Margin = new Thickness(0, 16, 0, 0)
        };

        info.Add(new Label
        {
            Text = model.Country,
            FontSize = 13,
            TextColor = Color.FromArgb("#8fb3d9")
        });

        info.Add(new Label
        {
            Text = $"No. {model.Number}",
            FontSize = 13,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#b8d6ff")
        });

        info.Add(new Label
        {
            Text = $"Issued {model.IssueDate:dd.MM.yyyy}",
            FontSize = 13,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#b8d6ff")
        });

        info.Add(new Label
        {
            Text = $"Expiry {model.Expiry}",
            FontSize = 13,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#b8d6ff")
        });

        grid.Add(info, 0, 1);

        //==========================
        // BUTTONS
        //==========================

        var buttons = new HorizontalStackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.End
        };

        var viewButton = new Button
        {
            Text = "👁",
            WidthRequest = 46,
            HeightRequest = 46,
            BackgroundColor = Color.FromArgb("#00bfff"),
            TextColor = Colors.White,
            CornerRadius = 14
        };

        viewButton.Clicked += (s, e) => model.ViewAction?.Invoke();

        var editButton = new Button
        {
            Text = "✏️",
            WidthRequest = 46,
            HeightRequest = 46,
            BackgroundColor = Color.FromArgb("#ffb020"),
            TextColor = Colors.Black,
            CornerRadius = 14
        };

        editButton.Clicked += (s, e) => model.EditAction?.Invoke();

        var deleteButton = new Button
        {
            Text = "🗑",
            WidthRequest = 46,
            HeightRequest = 46,
            BackgroundColor = Color.FromArgb("#ff4d6d"),
            TextColor = Colors.White,
            CornerRadius = 14
        };

        deleteButton.Clicked += (s, e) => model.DeleteAction?.Invoke();

        buttons.Add(viewButton);
        buttons.Add(editButton);
        buttons.Add(deleteButton);

        grid.Add(buttons, 0, 2);

        border.Content = grid;

        return border;
    }
}