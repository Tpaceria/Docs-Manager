using Microsoft.Maui.Controls;

namespace Docs_Manager.Controls;

public partial class DateSelector : ContentView
{
    public DateSelector()
    {
        InitializeComponent();

        LoadPickers();

        SelectedDate = DateTime.Today;
    }

    public DateTime SelectedDate
    {
        get
        {
            try
            {
                return new DateTime(
                    int.Parse(
                        YearPicker.SelectedItem?.ToString() ?? DateTime.Today.Year.ToString()),

                    int.Parse(
                        MonthPicker.SelectedItem?.ToString() ?? DateTime.Today.Month.ToString()),

                    int.Parse(
                        DayPicker.SelectedItem?.ToString() ?? DateTime.Today.Day.ToString()));
            }
            catch
            {
                return DateTime.Today;
            }
        }

        set
        {
            DayPicker.SelectedItem =
                value.Day.ToString("00");

            MonthPicker.SelectedItem =
                value.Month.ToString("00");

            YearPicker.SelectedItem =
                value.Year.ToString();
        }
    }

    private void LoadPickers()
    {
        for (int day = 1; day <= 31; day++)
        {
            DayPicker.Items.Add(
                day.ToString("00"));
        }

        for (int month = 1; month <= 12; month++)
        {
            MonthPicker.Items.Add(
                month.ToString("00"));
        }

        for (int year = 1980; year <= 2100; year++)
        {
            YearPicker.Items.Add(
                year.ToString());
        }
    }
}