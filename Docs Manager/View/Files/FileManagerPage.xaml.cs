using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class FileManagerPage : ContentView
{
    private MainPage? _mainPage;
    private DatabaseService? _database;
    private ObservableCollection<FileRecord> _allFiles = new();
    public ObservableCollection<FileRecord> Files { get; set; } = new();
    private string _selectedCategory = "ALL";

    public FileManagerPage(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage;

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        InitializeCategories();
        BindingContext = this;
        _ = LoadFilesAsync();
    }

    private void InitializeCategories()
    {
        var categories = new List<string>
        {
            "ALL",
            "Сертификаты",
            "Документы",
            "Медицина",
            "Опыт",
            "Другое"
        };

        CategoryPicker.ItemsSource = categories;
        CategoryPicker.SelectedIndex = 0;
    }

    private async Task LoadFilesAsync()
    {
        try
        {
            _allFiles.Clear();
            Files.Clear();

            var files = await _database!.GetAllFilesAsync();
            
            foreach (var file in files)
            {
                _allFiles.Add(file);
            }

            ApplyFilter();
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Ошибка",
                $"Ошибка загрузки файлов: {ex.Message}",
                "OK");
        }
    }

    private void OnCategoryChanged(object sender, EventArgs e)
    {
        var selectedItem = CategoryPicker.SelectedItem as string;
        _selectedCategory = selectedItem ?? "ALL";
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Files.Clear();

        IEnumerable<FileRecord> filtered = _allFiles;

        if (_selectedCategory != "ALL")
        {
            filtered = filtered.Where(f => f.Category == _selectedCategory);
        }

        foreach (var file in filtered)
        {
            Files.Add(file);
        }

        EmptyStateLayout.IsVisible = Files.Count == 0;
    }

    private async void OnAddFileClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync();

            if (result == null)
                return;

            // Выбор категории
            var categories = new[] { "Сертификаты", "Документы", "Медицина", "Опыт", "Другое" };
            var selectedCategory = await Application.Current!.MainPage!.DisplayActionSheet(
                "Выберите категорию файла",
                "Отмена",
                null,
                categories);

            if (selectedCategory == null || selectedCategory == "Отмена")
                return;

            // Ввод пользовательского имени файла
            var fileName = await Application.Current!.MainPage!.DisplayPromptAsync(
                "Имя файла",
                "Введите имя для файла (без расширения):",
                keyboard: Keyboard.Default);

            if (string.IsNullOrWhiteSpace(fileName))
                return;

            // Копирование файла и сохранение информации
            var fileInfo = new FileInfo(result.FullPath);
            var appDataPath = FileSystem.CacheDirectory;
            var categoryFolder = Path.Combine(appDataPath, selectedCategory);

            // Создание папки категории если её нет
            if (!Directory.Exists(categoryFolder))
                Directory.CreateDirectory(categoryFolder);

            var extension = fileInfo.Extension;
            var formattedName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
            var newFilePath = Path.Combine(categoryFolder, formattedName);

            // Копирование файла
            File.Copy(result.FullPath, newFilePath, overwrite: true);

            // Сохранение информации о файле в БД
            var fileRecord = new FileRecord
            {
                Category = selectedCategory,
                OriginalFileName = result.FileName,
                FormattedFileName = formattedName,
                FilePath = newFilePath,
                FileSize = fileInfo.Length / 1024, // KB
                FileExtension = extension,
                DateAdded = DateTime.Now,
                Description = ""
            };

            await _database!.SaveFileRecordAsync(fileRecord);
            await LoadFilesAsync();

            await Application.Current!.MainPage!.DisplayAlert(
                "Успешно",
                $"Файл добавлен: {formattedName}",
                "OK");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Ошибка",
                $"Ошибка при добавлении файла: {ex.Message}",
                "OK");
        }
    }

    private async void OnViewFileClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is FileRecord file)
        {
            try
            {
                if (!File.Exists(file.FilePath))
                {
                    await Application.Current!.MainPage!.DisplayAlert(
                        "Ошибка",
                        "Файл не найден на устройстве",
                        "OK");
                    return;
                }

                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(file.FilePath)
                });
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Ошибка",
                    $"Не удалось открыть файл: {ex.Message}",
                    "OK");
            }
        }
    }

    private async void OnEditFileClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is FileRecord file)
        {
            var newName = await Application.Current!.MainPage!.DisplayPromptAsync(
                "Редактировать имя файла",
                "Введите новое имя:",
                initialValue: Path.GetFileNameWithoutExtension(file.FormattedFileName));

            if (string.IsNullOrWhiteSpace(newName))
                return;

            try
            {
                var extension = file.FileExtension;
                var oldPath = file.FilePath;
                var newFileName = $"{newName}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
                var newPath = Path.Combine(Path.GetDirectoryName(oldPath)!, newFileName);

                File.Move(oldPath, newPath, overwrite: true);

                file.FormattedFileName = newFileName;
                file.FilePath = newPath;

                await _database!.SaveFileRecordAsync(file);
                await LoadFilesAsync();

                await Application.Current!.MainPage!.DisplayAlert(
                    "Успешно",
                    "Файл переименован",
                    "OK");
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Ошибка",
                    $"Ошибка при переименовании: {ex.Message}",
                    "OK");
            }
        }
    }

    private async void OnDeleteFileClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is FileRecord file)
        {
            var confirm = await Application.Current!.MainPage!.DisplayAlert(
                "Удалить файл",
                $"Вы уверены, что хотите удалить '{file.OriginalFileName}'?",
                "Да",
                "Нет");

            if (!confirm)
                return;

            try
            {
                await _database!.DeleteFileRecordAsync(file);
                await LoadFilesAsync();

                await Application.Current!.MainPage!.DisplayAlert(
                    "Успешно",
                    "Файл удален",
                    "OK");
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Ошибка",
                    $"Ошибка при удалении: {ex.Message}",
                    "OK");
            }
        }
    }
}