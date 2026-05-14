using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class ProfilePage : ContentPage
{
    private DatabaseService? _database;

    public ProfilePage()
    {
        InitializeComponent();

        FullNameLabel.Text = "TEST NAME";
        RankLabel.Text = "TEST RANK";
        NationalityLabel.Text = "TEST COUNTRY";

        TotalDocsLabel.Text = "99";
        ActiveDocsLabel.Text = "88";
        ExpiringDocsLabel.Text = "77";
        ExpiredDocsLabel.Text = "66";

        CrewStatusLabel.Text = "TEST STATUS";
    }
    private DatabaseService GetDatabase()
    {
        _database ??= ServiceHelper.GetService<DatabaseService>();
        return _database;
    }

    public async Task InitializeAsync()
    {
        await LoadProfileData();
        await LoadStatistics();
    }
    // =====================================================
    // PROFILE
    // =====================================================

    private async Task LoadProfileData()
    {
        try
        {
            var profile =
                await GetDatabase().GetUserProfileAsync();

            // DEBUG
            await DisplayAlert(
                "DEBUG PROFILE",
                profile == null
                    ? "PROFILE NULL"
                    : $"Loaded: {profile.FirstName} {profile.LastName}",
                "OK");

            if (profile == null)
                return;

            // FULL NAME
            FullNameLabel.Text =
                $"{profile.FirstName} {profile.LastName}".Trim();

            // RANK
            RankLabel.Text =
                string.IsNullOrWhiteSpace(profile.Position)
                    ? "Rank not specified"
                    : profile.Position;

            // NATIONALITY
            NationalityLabel.Text =
                string.IsNullOrWhiteSpace(profile.Citizenship)
                    ? "Not specified"
                    : profile.Citizenship;

            // PHOTO
            if (!string.IsNullOrWhiteSpace(profile.PhotoPath) &&
                File.Exists(profile.PhotoPath))
            {
                ProfilePhoto.Source =
                    new FileImageSource
                    {
                        File = profile.PhotoPath
                    };
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                $"Failed to load profile: {ex.Message}",
                "OK");
        }
    }

    // =====================================================
    // STATS
    // =====================================================

    private async Task LoadStatistics()
    {
        try
        {
            var certificates =
                await GetDatabase().GetCertificatesAsync();

            // DEBUG
            await DisplayAlert(
                "DEBUG CERTIFICATES",
                $"Certificates found: {certificates.Count}",
                "OK");

            int total =
                certificates.Count;

            int active =
                certificates.Count(c =>
                    !c.IsLifetime &&
                    !c.IsExpired &&
                    !c.IsExpiringSoon);

            int expiring =
                certificates.Count(c =>
                    c.IsExpiringSoon);

            int expired =
                certificates.Count(c =>
                    c.IsExpired);

            // LABELS
            TotalDocsLabel.Text =
                total.ToString();

            ActiveDocsLabel.Text =
                active.ToString();

            ExpiringDocsLabel.Text =
                expiring.ToString();

            ExpiredDocsLabel.Text =
                expired.ToString();

            // READINESS STATUS
            if (expired > 0)
            {
                CrewStatusLabel.Text = "NOT READY";
            }
            else if (expiring > 0)
            {
                CrewStatusLabel.Text = "RENEWAL REQUIRED";
            }
            else
            {
                CrewStatusLabel.Text = "READY";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                $"Failed to load statistics: {ex.Message}",
                "OK");
        }
    }
}