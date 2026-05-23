using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Docs_Manager.View;

public partial class OtherPage : ContentView
{
    private DatabaseService _database;
    private ObservableCollection<Certificate> _allCertificates = new();

    public ObservableCollection<Certificate> Certificates { get; set; } = new();

    private MainPage _mainPage;

    public OtherPage()
    {
        InitializeComponent();

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        CertificateCollectionView.ItemsSource = Certificates;

        _ = LoadCertificates();
    }

    public OtherPage(MainPage mainPage) : this()
    {
        _mainPage = mainPage;
    }

    private async Task LoadCertificates()
    {
        _allCertificates.Clear();
        Certificates.Clear();

        var list = await _database.GetCertificatesAsync();

        foreach (var cert in list.Where(c => c.Category == "OTHER"))
        {
            _allCertificates.Add(cert);
        }

        foreach (var cert in _allCertificates)
        {
            Certificates.Add(cert);
        }
    }

    private void OnAddCertificateClicked(object sender, EventArgs e)
    {
        var page = new AddOtherPage(this, _mainPage);

        _mainPage.SetPage(page);
    }

    private void OnEditCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Certificate cert)
        {
            var page = new AddOtherPage(cert, this, _mainPage);

            _mainPage.SetPage(page);
        }
    }

    private async void OnViewCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Certificate cert)
        {
            if (string.IsNullOrWhiteSpace(cert.FilePath) ||
                !File.Exists(cert.FilePath))
            {
                bool attachNow =
                    await Application.Current.MainPage.DisplayAlert(
                        "Файл не найден",
                        "У записи нет прикрепленного файла.\nПрикрепить сейчас?",
                        "Прикрепить",
                        "Отмена");

                if (!attachNow)
                    return;

                var result = await FilePicker.Default.PickAsync();

                if (result != null)
                {
                    cert.FilePath = result.FullPath;

                    await _database.SaveCertificateAsync(cert);

                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(cert.FilePath)
                    });
                }

                return;
            }

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(cert.FilePath)
            });
        }
    }

    private async void OnDeleteCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Certificate cert)
        {
            bool confirm =
                await Application.Current.MainPage.DisplayAlert(
                    "Delete",
                    $"Delete \"{cert.Document}\"?",
                    "Yes",
                    "Cancel");

            if (!confirm)
                return;

            await _database.DeleteCertificateAsync(cert);
            await LoadCertificates();
        }
    }

    public async void AddCertificate(Certificate cert)
    {
        await _database.SaveCertificateAsync(cert);

        await LoadCertificates();
    }

    public async void RefreshList()
    {
        await LoadCertificates();
    }
}