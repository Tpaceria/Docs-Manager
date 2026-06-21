using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddBiometricPage : ContentPage
{
    private readonly DatabaseService _database;

    public AddBiometricPage()
    {
        InitializeComponent();

        _database =
            ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException();

        _ = LoadBiometricAsync();
    }

    private async Task LoadBiometricAsync()
    {
        var biometric =
            (await _database.GetBiometricAsync())
            .FirstOrDefault();

        if (biometric == null)
            return;

        HeightEntry.Text =
            biometric.Height.ToString();

        WeightEntry.Text =
            biometric.Weight.ToString();

        ShoeSizeEntry.Text =
            biometric.ShoeSize.ToString();

        OverallSizeEntry.Text =
            biometric.OverallSize.ToString();

        HairColorPicker.SelectedItem =
            biometric.HairColor;

        EyeColorPicker.SelectedItem =
            biometric.EyeColor;
    }

    private async void OnSaveClicked(
        object sender,
        EventArgs e)
    {
        var oldData =
            await _database.GetBiometricAsync();

        foreach (var item in oldData)
        {
            await _database.DeleteBiometricAsync(item);
        }

        await _database.SaveBiometricAsync(
            new BiometricModel
            {
                Height =
                    int.TryParse(
                        HeightEntry.Text,
                        out var height)
                        ? height
                        : 0,

                Weight =
                    int.TryParse(
                        WeightEntry.Text,
                        out var weight)
                        ? weight
                        : 0,

                ShoeSize =
                    int.TryParse(
                        ShoeSizeEntry.Text,
                        out var shoe)
                        ? shoe
                        : 0,

                OverallSize =
                    int.TryParse(
                        OverallSizeEntry.Text,
                        out var overall)
                        ? overall
                        : 0,

                HairColor =
    HairColorPicker.SelectedItem?
        .ToString() ?? "",

                EyeColor =
    EyeColorPicker.SelectedItem?
        .ToString() ?? ""
            });

        await DisplayAlert(
            "Saved",
            "Biometric data saved successfully",
            "OK");

        await Navigation.PopModalAsync();
    }

    private async void OnCloseClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}