namespace Docs_Manager.View;

public partial class NotesPage : ContentView
{
    private MainPage? _mainPage;

    public NotesPage(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage;
    }

    private void OnAddNoteClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.DisplayAlert(
            "Добавить заметку",
            "Добавление заметок - Coming soon",
            "OK");
    }
}