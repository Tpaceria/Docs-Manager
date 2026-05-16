namespace Docs_Manager.View;

public partial class FilesPage : ContentView
{
    private readonly MainPage _mainPage;

    public FilesPage(MainPage mainPage)
    {
        InitializeComponent();

        _mainPage = mainPage;
    }

    void OnCertificatesClicked(object sender, EventArgs e)
    {
        _mainPage.SetPage(
            new CertificatePage(_mainPage));
    }

    void OnCocClicked(object sender, EventArgs e)
    {
        _mainPage.SetPage(
            new CocEndorsementPage(_mainPage));
    }

    void OnDocumentsClicked(object sender, EventArgs e)
    {
        _mainPage.SetPage(
            new DocumentsPage(_mainPage));
    }

    void OnMedicineClicked(object sender, EventArgs e)
    {
        _mainPage.SetPage(
            new MedicinePage(_mainPage));
    }

    void OnOtherClicked(object sender, EventArgs e)
    {
        _mainPage.SetPage(
            new OtherPage(_mainPage));
    }
}