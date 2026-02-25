using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _database = null!;

    public ObservableCollection<Document> Documents { get; set; } = new();

    public MainPage(DatabaseService database)
    {
        InitializeComponent();

        _database = database;

        // Устанавливаем BindingContext
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Documents.Clear();

        var docs = await _database.GetDocumentsAsync();

        foreach (var doc in docs)
            Documents.Add(doc);
    }

    private async void OnAddDocumentClicked(object? sender, EventArgs e)
    {
        var newDoc = new Document
        {
            Title = "Test Certificate",
            Type = "STCW",
            Number = "123456",
            IssueDate = DateTime.Today,
            ExpiryDate = DateTime.Today.AddMonths(6),
            FilePath = ""
        };

        await _database.SaveDocumentAsync(newDoc);

        Documents.Add(newDoc);
    }
}