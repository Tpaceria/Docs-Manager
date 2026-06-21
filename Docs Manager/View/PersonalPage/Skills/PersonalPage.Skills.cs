using System;
using System.Collections.Generic;
using System.Text;

namespace Docs_Manager.View;

public partial class PersonalPage
{
    private async void OnEditSkillsClicked(
    object sender,
    EventArgs e)
{
    var page =
        new AddSkillsPage();

    page.Disappearing += async (s, e2) =>
    {
        await LoadSkillsPreview();
    };

    await Navigation.PushModalAsync(page);
}

private async Task LoadSkillsPreview()
{
    try
    {
        var skills =
            await GetDatabase()
                .GetSkillsAsync();

        SkillsPreviewContainer.Clear();

        var skill =
            skills.FirstOrDefault();

        if (skill == null)
            return;

        SkillsPreviewContainer.Add(
            new Label
            {
                Text =
                    $"English Level: {skill.EnglishLevel}",
                TextColor = Colors.White
            });

        SkillsPreviewContainer.Add(
            new Label
            {
                Text =
                    $"Marlins Test: {skill.MarlinsScore}%",
                TextColor = Colors.White
            });

        SkillsPreviewContainer.Add(
            new Label
            {
                Text =
                    $"CES Score: {skill.CesScore}%",
                TextColor = Colors.White
            });

        SkillsPreviewContainer.Add(
            new Label
            {
                Text =
                    $"Languages: {skill.AdditionalLanguages}",
                TextColor = Colors.White
            });
    }
    catch
    {
    }
}
}
