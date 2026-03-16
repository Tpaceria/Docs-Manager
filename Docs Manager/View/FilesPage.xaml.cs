using Docs_Manager.Data;
using Docs_Manager.Models;
using Docs_Manager.Services;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class FilesPage : ContentPage
{
    private DatabaseService _database;
    private FileStorageService _fileStorage;
    private FileShareService _fileShare;

    public ObservableCollection<StoredFile> Files { get; set; } = new();

    public FilesPage()
    {
        InitializeComponent();

        _database = Application.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetService<DatabaseService>()!;

        _fileStorage = new FileStorageService(_database);
        _fileShare = new FileShareService();

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Files.Clear();
        var files = await _fileStorage.GetAllFilesAsync();

        foreach (var file in files)
            Files.Add(file);
    }

    private async void OnAddFileClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a file"
            });

            if (result != null)
            {
                var file = await _fileStorage.UploadFileAsync(result.FullPath);
                if (file != null)
                {
                    Files.Add(file);
                    await DisplayAlert("Success", "File uploaded successfully", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnShareClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is StoredFile file)
        {
            string action = await DisplayActionSheet("Share via:", "Cancel", null, "Email", "WhatsApp", "Local Share");

            bool result = action switch
            {
                "Email" => await _fileShare.ShareFileAsync(file.FilePath, FileShareService.ShareMethod.Email),
                "WhatsApp" => await _fileShare.ShareFileAsync(file.FilePath, FileShareService.ShareMethod.WhatsApp),
                "Local Share" => await _fileShare.ShareFileAsync(file.FilePath, FileShareService.ShareMethod.LocalShare),
                _ => false
            };

            if (result)
                await DisplayAlert("Success", "Shared successfully", "OK");
            else
                await DisplayAlert("Error", "Failed to share", "OK");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is StoredFile file)
        {
            bool confirm = await DisplayAlert("Confirm", "Delete this file?", "Yes", "No");

            if (confirm)
            {
                bool success = await _fileStorage.DeleteFileAsync(file);
                if (success)
                {
                    Files.Remove(file);
                    await DisplayAlert("Success", "File deleted", "OK");
                }
            }
        }
    }
}