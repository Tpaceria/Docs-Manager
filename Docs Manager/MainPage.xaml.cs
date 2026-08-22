using Docs_Manager.View;

namespace Docs_Manager;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        ShowPersonal();
    }

    public void SetPage(IView view)
    {
        ContentArea.Children.Clear();

        if (view is Microsoft.Maui.Controls.View mauiView)
        {
            ContentArea.Children.Add(mauiView);
        }
    }
    private void ShowPersonal()
    {
        SetPage(new PersonalPage());
    }

    private void ShowCertificates()
    {
        SetPage(new CertificatePage(this));
    }

    private void ShowCoc()
    {
        SetPage(new CocEndorsementPage(this));
    }

    private void ShowDocuments()
    {
        SetPage(new DocumentsPage(this));
    }

    private void ShowMedicine()
    {
        SetPage(new MedicinePage(this));
    }

    private void ShowOther()
    {
        SetPage(new OtherPage(this));
    }

    private void ShowExperience()
    {
        SetPage(new ExperiencePage(this));
    }

    private void ShowCw()
    {
        // TODO: Create CwPage
        DisplayAlert("CW", "CW Page - Coming soon", "OK");
    }

    private void ShowSend()
    {
        // TODO: Create SendPage with checklist for email/print/other actions
        DisplayAlert("Отправить", "Send documents - Coming soon\n\nChecklist:\n- Email\n- Print\n- Other actions", "OK");
    }

    private void ShowFiles()
    {
        // TODO: Create FilesPage for managing attached files with folder distribution
        DisplayAlert("Файлы", "File management - Coming soon\n\nFeatures:\n- Organize files\n- Create folders\n- Rename with standard format", "OK");
    }

    private void ShowNotes()
    {
        // TODO: Create NotesPage for notes/comments
        DisplayAlert("Заметки", "Notes - Coming soon", "OK");
    }

    private void OnPersonalClicked(object sender, EventArgs e)
    {
        ShowPersonal();
    }

    private void OnCertificatesClicked(object sender, EventArgs e)
    {
        ShowCertificates();
    }

    private void OnCocClicked(object sender, EventArgs e)
    {
        ShowCoc();
    }

    private void OnDocumentsClicked(object sender, EventArgs e)
    {
        ShowDocuments();
    }

    private void OnMedicineClicked(object sender, EventArgs e)
    {
        ShowMedicine();
    }

    private void OnOtherClicked(object sender, EventArgs e)
    {
        ShowOther();
    }

    private void OnExperienceClicked(object sender, EventArgs e)
    {
        ShowExperience();
    }

    private void OnCwClicked(object sender, EventArgs e)
    {
        ShowCw();
    }

    private void OnSendClicked(object sender, EventArgs e)
    {
        ShowSend();
    }

    private void OnFilesClicked(object sender, EventArgs e)
    {
        ShowFiles();
    }

    private void OnNotesClicked(object sender, EventArgs e)
    {
        ShowNotes();
    }
}
