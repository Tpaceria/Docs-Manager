using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class FilesPage : ContentPage
{
    private readonly DatabaseService _database;
    private string _currentCategory = "certificates";

    public ObservableCollection<StoredFile> Files { get; set; } = new();

    public FilesPage()
    {
        InitializeComponent();

        _database = Application.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetService<DatabaseService>()!;

        FilesCollection.ItemsSource = Files;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        UpdateAddButtonText();
        await LoadFilesAsync();
    }

    private async Task LoadFilesAsync()
    {
        Files.Clear();
        var list = await _database.GetFilesByCategoryAsync(_currentCategory);

        foreach (var file in list)
            Files.Add(file);
    }

    private async void OnAddFileClicked(object sender, EventArgs e)
    {
        ContentPage page = _currentCategory switch
        {
            "certificates" => new AddCertificatePage(),
            "coc" => new AddCocPage(),
            "documents" => new AddDocumentPage(),
            "medicine" => new AddMedicinePage(),
            "other" => new AddOtherPage(),
            _ => new AddCertificatePage()
        };

        await Navigation.PushAsync(page);
    }
    private async void OnCategoryButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            // Преобразуй текст кнопки в категорию
            string buttonText = btn.Text ?? "";

            _currentCategory = buttonText.ToLower() switch
            {
                "certificates" => "certificates",
                "coc & endorsement" => "coc",
                "documents" => "documents",
                "medicine" => "medicine",
                "other" => "other",
                _ => "certificates"
            };

            // Обнови текст кнопки Add
            UpdateAddButtonText();

            // Обнови цвета кнопок
            UpdateButtonColors();

            // Загрузи файлы новой категории
            await LoadFilesAsync();
        }
    }

    private void UpdateAddButtonText()
    {
        string categoryText = _currentCategory switch
        {
            "certificates" => "Certificate",
            "coc" => "COC & Endorsement",
            "documents" => "Document",
            "medicine" => "Medicine",
            "other" => "File",
            _ => "File"
        };

        AddButton.Text = $"+ Add {categoryText}";
    }

    private void UpdateButtonColors()
    {
        // Получи HorizontalStackLayout с кнопками (Grid.Row="1")
        if (this.Content is Grid grid)
        {
            foreach (var child in grid.Children)
            {
                if (child is HorizontalStackLayout hsl)
                {
                    // Пройди по всем кнопкам в HorizontalStackLayout
                    foreach (var button in hsl.Children.OfType<Button>())
                    {
                        // Определи категорию кнопки
                        string btnCategory = button.Text?.ToLower() switch
                        {
                            "certificates" => "certificates",
                            "coc & endorsement" => "coc",
                            "documents" => "documents",
                            "medicine" => "medicine",
                            "other" => "other",
                            _ => ""
                        };

                        // Установи цвет активной кнопки
                        button.TextColor = btnCategory == _currentCategory
                            ? Color.FromArgb("#0099CC")
                            : Color.FromArgb("#666666");

                        button.FontAttributes = btnCategory == _currentCategory
                            ? FontAttributes.Bold
                            : FontAttributes.None;
                    }
                }
            }
        }
    }

    private async void OnDownloadFile(object sender, TappedEventArgs e)
    {
        if (e.Parameter is StoredFile file)
        {
            await DisplayAlertAsync("Download", $"Загрузить: {file.FileName}?", "OK", "Cancel");
        }
    }

    private async void OnDeleteFile(object sender, TappedEventArgs e)
    {
        if (e.Parameter is StoredFile file)
        {
            bool answer = await DisplayAlertAsync("Delete", $"Удалить {file.FileName}?", "Yes", "No");
            if (answer)
            {
                await _database.DeleteFileAsync(file);
                Files.Remove(file);
            }
        }
    }
}