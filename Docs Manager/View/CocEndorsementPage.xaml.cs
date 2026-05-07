using Docs_Manager.Data;
using Docs_Manager.Models;
using System.Collections.ObjectModel;

namespace Docs_Manager.View;

public partial class CocEndorsementPage : ContentPage
{
    private DatabaseService _database;

    private ObservableCollection<Certificate> _allCertificates = new();

    public ObservableCollection<Certificate> Certificates { get; set; } = new();

    private MainPage _mainPage;

    public CocEndorsementPage()
    {
        InitializeComponent();

        _database = ServiceHelper.GetService<DatabaseService>()
            ?? throw new InvalidOperationException("DatabaseService not found");

        CertificateCollectionView.ItemsSource = Certificates;

        _ = LoadCertificates();
    }

    public CocEndorsementPage(MainPage mainPage) : this()
    {
        _mainPage = mainPage;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadCertificates();
    }

    private async Task LoadCertificates()
    {
        _allCertificates.Clear();
        Certificates.Clear();

        var list = await _database.GetCertificatesAsync();

        foreach (var cert in list.Where(c => c.Category == "COC"))
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
        var page = new AddCocPage(this, _mainPage);

        _mainPage.SetPage(page);
    }

    private void OnEditCertificateClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Certificate cert)
        {
            var page = new AddCocPage(cert, this, _mainPage);

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
                    await Application.Current.Windows[0].Page.DisplayAlertAsync(
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
                await Application.Current.Windows[0].Page.DisplayAlertAsync(
                    "Delete",
                    $"Delete \"{cert.Document}\"?",
                    "Yes",
                    "Cancel");

            if (!confirm)
                return;

            await _database.DeleteAsync(cert);

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