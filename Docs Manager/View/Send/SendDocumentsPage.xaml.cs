using Docs_Manager.Models;
using Docs_Manager.Services;

namespace Docs_Manager.View;

public partial class SendDocumentsPage : ContentView
{
    private readonly MainPage _mainPage;
    private readonly HashSet<StoredFile> _selected = new();

    public SendDocumentsPage(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage;
        _ = LoadFilesAsync();
    }

    private async Task LoadFilesAsync()
    {
        try
        {
            var storage = ServiceHelper.GetService<FileStorageService>();
            var files = (await storage.GetAllFilesAsync())
                .Where(f => File.Exists(f.FilePath))
                .ToList();

            FilesView.ItemsSource = files;
            EmptyLabel.IsVisible = files.Count == 0;
            FilesView.IsVisible = files.Count > 0;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Load files error: {ex.Message}");
            EmptyLabel.IsVisible = true;
        }
    }

    private void OnFileCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (sender is not CheckBox { BindingContext: StoredFile file })
            return;

        if (e.Value)
            _selected.Add(file);
        else
            _selected.Remove(file);
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        if (_selected.Count == 0)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "Please select at least one file", "OK");
            return;
        }

        _mainPage.SetPage(new SendMethodPage(_mainPage, _selected.ToList()));
    }
}
