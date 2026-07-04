using Microsoft.Maui.Controls;

namespace Docs_Manager.View;

public partial class PersonalPage
{
    private void BuildTable(
        VerticalStackLayout container,
        List<Grid> rows)
    {
        container.Clear();

        for (int i = 0; i < rows.Count; i++)
        {
            var stack = new VerticalStackLayout
            {
                Spacing = 0
            };

            stack.Add(rows[i]);

            if (i < rows.Count - 1)
            {
                stack.Add(new BoxView
                {
                    HeightRequest = 1,
                    BackgroundColor = Color.FromArgb("#224b75"),
                    Opacity = 0.6,
                    Margin = new Thickness(0, 4)
                });
            }

            container.Add(stack);
        }
    }

    // =====================================
    // 3 Columns (COC / Certificates)
    // =====================================

    private Grid CreateThreeColumnRow(
        string first,
        string second,
        string third,
        Color thirdColor)
    {
        var row = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(160),
                new ColumnDefinition(120)
            },

            Padding = new Thickness(0, 4)
        };

        row.Add(new Label
        {
            Text = first,
            TextColor = Colors.White,
            FontSize = 14,
            LineBreakMode = LineBreakMode.TailTruncation
        }, 0, 0);

        row.Add(new Label
        {
            Text = second,
            TextColor = Colors.White,
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.End
        }, 1, 0);

        row.Add(new Label
        {
            Text = third,
            TextColor = thirdColor,
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.End
        }, 2, 0);

        return row;
    }

    // =====================================
    // 5 Columns (Experience)
    // =====================================

    private Grid CreateFiveColumnRow(
        string vessel,
        string rank,
        string vesselType,
        string dwt,
        string period,
        Color? periodColor = null)
    {
        var row = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star), // Vessel
                new ColumnDefinition(150),             // Rank
                new ColumnDefinition(170),             // Type of Vessel
                new ColumnDefinition(90),              // DWT
                new ColumnDefinition(120)              // Period
            },

            Padding = new Thickness(0, 4)
        };

        row.Add(new Label
        {
            Text = vessel,
            TextColor = Colors.White,
            FontSize = 14,
            LineBreakMode = LineBreakMode.TailTruncation
        }, 0, 0);

        row.Add(new Label
        {
            Text = rank,
            TextColor = Colors.White,
            FontSize = 14
        }, 1, 0);

        row.Add(new Label
        {
            Text = vesselType,
            TextColor = Colors.White,
            FontSize = 14,
            LineBreakMode = LineBreakMode.TailTruncation
        }, 2, 0);

        row.Add(new Label
        {
            Text = dwt,
            TextColor = Colors.White,
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.End
        }, 3, 0);

        row.Add(new Label
        {
            Text = period,
            TextColor = periodColor ?? Color.FromArgb("#00d4ff"),
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.End
        }, 4, 0);

        return row;
    }
}