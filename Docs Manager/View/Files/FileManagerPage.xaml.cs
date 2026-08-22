namespace Docs_Manager.View;

public partial class FileManagerPage : ContentView
{
    private MainPage? _mainPage;

    public FileManagerPage(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage;
    }
}