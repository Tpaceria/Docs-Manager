using Docs_Manager.View;
using System.Diagnostics;

namespace Docs_Manager;

public partial class MainPage : ContentPage
{
    private ContentPage _currentPage;

    public MainPage()
    {
        InitializeComponent();
        ShowPersonal();
    }

    private void ShowPersonal()
    {
        ResetButtons();
        PersonalBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        PersonalBtn.TextColor = Color.FromArgb("#00d4ff");
        SetPage(new PersonalPage());
    }

    private void ShowCertificates()
    {
        ResetButtons();
        CertificatesBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        CertificatesBtn.TextColor = Color.FromArgb("#00d4ff");

        // ✅ ПЕРЕДАЁМ Navigation в конструктор
        SetPage(new CertificatePage(this.Navigation));
    }

    private void ShowCoc()
    {
        ResetButtons();
        CocBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        CocBtn.TextColor = Color.FromArgb("#00d4ff");

        // ✅ ПЕРЕДАЁМ Navigation в конструктор
        SetPage(new CocEndorsementPage(this.Navigation));
    }

    private void ShowDocuments()
    {
        ResetButtons();
        DocumentsBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        DocumentsBtn.TextColor = Color.FromArgb("#00d4ff");

        // ✅ ПЕРЕДАЁМ Navigation в конструктор
        SetPage(new DocumentsPage(this.Navigation));
    }

    private void ShowMedicine()
    {
        ResetButtons();
        MedicineBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        MedicineBtn.TextColor = Color.FromArgb("#00d4ff");

        // ✅ ПЕРЕДАЁМ Navigation в конструктор
        SetPage(new MedicinePage(this.Navigation));
    }

    private void ShowOther()
    {
        ResetButtons();
        OtherBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        OtherBtn.TextColor = Color.FromArgb("#00d4ff");

        // ✅ ПЕРЕДАЁМ Navigation в конструктор
        SetPage(new OtherPage(this.Navigation));
    }

    private void ShowExperience()
    {
        ResetButtons();
        ExperienceBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        ExperienceBtn.TextColor = Color.FromArgb("#00d4ff");

        // ✅ ПЕРЕДАЁМ Navigation в конструктор
        SetPage(new ExperiencePage(this.Navigation));
    }

    private void SetPage(ContentPage page)
    {
        Debug.WriteLine($"🔵 SetPage: {page.GetType().Name}");

        _currentPage = page;
        ContentArea.Content = page.Content;
        page.SendAppearing();

        Debug.WriteLine($"✅ Page set: {page.GetType().Name}");
    }

    private void ResetButtons()
    {
        PersonalBtn.BackgroundColor = Colors.Transparent;
        PersonalBtn.TextColor = Color.FromArgb("#a8b8cc");
        CertificatesBtn.BackgroundColor = Colors.Transparent;
        CertificatesBtn.TextColor = Color.FromArgb("#a8b8cc");
        CocBtn.BackgroundColor = Colors.Transparent;
        CocBtn.TextColor = Color.FromArgb("#a8b8cc");
        DocumentsBtn.BackgroundColor = Colors.Transparent;
        DocumentsBtn.TextColor = Color.FromArgb("#a8b8cc");
        MedicineBtn.BackgroundColor = Colors.Transparent;
        MedicineBtn.TextColor = Color.FromArgb("#a8b8cc");
        OtherBtn.BackgroundColor = Colors.Transparent;
        OtherBtn.TextColor = Color.FromArgb("#a8b8cc");
        ExperienceBtn.BackgroundColor = Colors.Transparent;
        ExperienceBtn.TextColor = Color.FromArgb("#a8b8cc");
    }

    private void OnPersonalClicked(object sender, EventArgs e) => ShowPersonal();
    private void OnCertificatesClicked(object sender, EventArgs e) => ShowCertificates();
    private void OnCocClicked(object sender, EventArgs e) => ShowCoc();
    private void OnDocumentsClicked(object sender, EventArgs e) => ShowDocuments();
    private void OnMedicineClicked(object sender, EventArgs e) => ShowMedicine();
    private void OnOtherClicked(object sender, EventArgs e) => ShowOther();
    private void OnExperienceClicked(object sender, EventArgs e) => ShowExperience();
}