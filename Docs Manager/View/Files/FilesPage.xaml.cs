namespace Docs_Manager.View;

public partial class FilesPage : ContentView
{
    private MainPage? _mainPage;

    public FilesPage(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage;
    }
}