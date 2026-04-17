namespace Docs_Manager;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new MainPage())
        {
            BarBackgroundColor = Color.FromArgb("#0f1f2e"),
            BarTextColor = Color.FromArgb("#00d4ff")
        };
    }
}