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
}