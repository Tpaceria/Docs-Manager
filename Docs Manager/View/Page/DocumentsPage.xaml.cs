using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class DocumentsPage : ContentView
{
    private readonly DatabaseService _database;

    private readonly ObservableCollection<Document> _allDocuments = new();

    public ObservableCollection<Document> Documents { get; set; } = new();

    private MainPage _mainPage;

    public DocumentsPage()
    {
        InitializeComponent();

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        DocumentsCollectionView.ItemsSource = Documents;

        _ = LoadDocuments();
    }

    public DocumentsPage(MainPage mainPage)
        : this()
    {
        _mainPage = mainPage;
    }

    private async Task LoadDocuments()
    {
        _allDocuments.Clear();
        Documents.Clear();

        var list = await _database.GetDocumentsAsync();

        foreach (var document in list)
        {
            _allDocuments.Add(document);
        }

        foreach (var document in _allDocuments)
        {
            Documents.Add(document);
        }
    }

    private void OnAddDocumentClicked(object sender, EventArgs e)
    {
        var page = new AddDocumentPage(this, _mainPage);

        _mainPage.SetPage(page);
    }

    private void OnEditDocumentClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Document document)
        {
            var page = new AddDocumentPage(document, this, _mainPage);

            _mainPage.SetPage(page);
        }
    }

    private async void OnViewDocumentClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Document document)
        {
            if (string.IsNullOrWhiteSpace(document.FilePath) ||
                !File.Exists(document.FilePath))
            {
                bool attachNow =
                    await Application.Current.MainPage.DisplayAlert(
                        "File not found",
                        "No file is attached to this document.\nAttach one now?",
                        "Attach",
                        "Cancel");

                if (!attachNow)
                    return;

                var result = await FilePicker.Default.PickAsync();

                if (result != null)
                {
                    document.FilePath = result.FullPath;

                    await _database.SaveDocumentAsync(document);

                    await Launcher.OpenAsync(
                        new OpenFileRequest
                        {
                            File = new ReadOnlyFile(document.FilePath)
                        });
                }

                return;
            }

            await Launcher.OpenAsync(
                new OpenFileRequest
                {
                    File = new ReadOnlyFile(document.FilePath)
                });
        }
    }

    private async void OnDeleteDocumentClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Document document)
        {
            bool confirm =
                await Application.Current.MainPage.DisplayAlert(
                    "Delete",
                    $"Delete \"{document.Title}\"?",
                    "Yes",
                    "Cancel");

            if (!confirm)
                return;

            await _database.DeleteDocumentAsync(document);

            await LoadDocuments();
        }
    }

    public async void AddDocument(Document document)
    {
        await _database.SaveDocumentAsync(document);

        await LoadDocuments();
    }

    public async void RefreshList()
    {
        await LoadDocuments();
    }
}