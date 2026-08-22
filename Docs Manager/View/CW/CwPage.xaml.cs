namespace Docs_Manager.View;

public partial class CwPage : ContentView
{
    private MainPage? _mainPage;

    public CwPage(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage;
    }
}
