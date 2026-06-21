using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddSkillsPage : ContentPage
{
    private readonly DatabaseService _database;

    public AddSkillsPage()
    {
        InitializeComponent();

        _database =
            ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException();

        _ = LoadSkillsAsync();
    }

    private async Task LoadSkillsAsync()
    {
        var skill =
            (await _database.GetSkillsAsync())
            .FirstOrDefault();

        if (skill == null)
            return;

        EnglishLevelPicker.SelectedItem =
            skill.EnglishLevel;

        MarlinsEntry.Text =
            skill.MarlinsScore;

        CesEntry.Text =
            skill.CesScore;

        LanguagesEditor.Text =
            skill.AdditionalLanguages;
    }

    private async void OnSaveClicked(
        object sender,
        EventArgs e)
    {
        var oldSkills =
            await _database.GetSkillsAsync();

        foreach (var item in oldSkills)
        {
            await _database.DeleteSkillsAsync(item);
        }

        await _database.SaveSkillsAsync(
            new SkillsModel
            {
                EnglishLevel =
                    EnglishLevelPicker.SelectedItem?.ToString() ?? "",

                MarlinsScore =
                    MarlinsEntry.Text ?? "",

                CesScore =
                    CesEntry.Text ?? "",

                AdditionalLanguages =
                    LanguagesEditor.Text ?? ""
            });

        await DisplayAlert(
            "Saved",
            "Skills saved successfully",
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