using Docs_Manager.Models;
using Docs_Manager.Services;

namespace Docs_Manager.View;

public partial class SendMethodPage : ContentView
{
    private readonly MainPage _mainPage;
    private readonly List<string> _paths;
    private readonly FileShareService _shareService;

    public SendMethodPage(MainPage mainPage, List<StoredFile> files)
    {
        InitializeComponent();
        _mainPage = mainPage;
        _paths = files.Select(f => f.FilePath).ToList();
        _shareService = ServiceHelper.GetService<FileShareService>();
        SummaryLabel.Text = $"Выбрано файлов: {files.Count}";
    }

    private async Task ShowErrorAsync(string message)
    {
        await Application.Current!.MainPage!.DisplayAlert("Error", message, "OK");
    }

    private async void OnEmailClicked(object sender, EventArgs e)
    {
        if (!await _shareService.EmailFilesAsync(_paths))
            await ShowErrorAsync("Unable to open email client");
    }

    private async void OnMessengerClicked(object sender, EventArgs e)
    {
        if (!await _shareService.ShareFilesAsync(_paths, "Send with"))
            await ShowErrorAsync("Unable to share files");
    }

    private async void OnArchiveClicked(object sender, EventArgs e)
    {
        var archive = await _shareService.CreateArchiveAsync(_paths);
        if (archive == null)
        {
            await ShowErrorAsync("Unable to create archive");
            return;
        }

        await _shareService.ShareFilesAsync(new[] { archive }, "Archive");
    }

    private async void OnUsbClicked(object sender, EventArgs e)
    {
        if (!await _shareService.ShareFilesAsync(_paths, "Save to USB"))
            await ShowErrorAsync("Unable to export files");
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        _mainPage.SetPage(new SendDocumentsPage(_mainPage));
    }
}
