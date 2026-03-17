using Docs_Manager.Data;
using Docs_Manager.Models;

namespace Docs_Manager.View;

public partial class AddCertificatePage : ContentPage
{
    private readonly DatabaseService _database;
    private string? _selectedFilePath;
    private string? _selectedFileName;

    public AddCertificatePage()
    {
        InitializeComponent();

        _database = Application.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetService<DatabaseService>()!;
    }

    private async void OnPickFileClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select a certificate file"
            });

            if (result != null)
            {
                _selectedFilePath = result.FullPath;
                _selectedFileName = result.FileName;

                // Получи размер файла
                var fileInfo = new FileInfo(_selectedFilePath);
                long fileSize = fileInfo.Length;

                // Покажи информацию о файле
                FileNameLabel.Text = _selectedFileName;
                FileSizeLabel.Text = $"Size: {FormatFileSize(fileSize)}";
                FileInfoStack.IsVisible = true;
                PickFileButton.Text = "✅ File Selected";
                PickFileButton.BackgroundColor = Color.FromArgb("#28A745");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Failed to pick file: {ex.Message}", "OK", "");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_selectedFilePath))
        {
            await DisplayAlertAsync("Error", "Please select a file first", "OK", "");
            return;
        }

        if (string.IsNullOrEmpty(CertificateNameEntry.Text))
        {
            await DisplayAlertAsync("Error", "Please enter a certificate name", "OK", "");
            return;
        }

        try
        {
            // Создай новый файл
            var file = new StoredFile
            {
                FileName = CertificateNameEntry.Text,
                Category = "certificates",
                FilePath = _selectedFilePath,
                Description = DescriptionEditor.Text ?? "",
                UploadDate = DateTime.Now,
                FileType = Path.GetExtension(_selectedFileName ?? ""),
                FileSizeBytes = new FileInfo(_selectedFilePath).Length
            };

            // Сохрани в БД
            await _database.SaveFileAsync(file);

            await DisplayAlertAsync("Success", "Certificate uploaded successfully!", "OK", "");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Failed to save: {ex.Message}", "OK", "");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}