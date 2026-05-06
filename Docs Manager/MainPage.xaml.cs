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

        // ✔ старый рабочий вариант
        SetPage(new CertificatePage(this));
    }

    private void ShowCoc()
    {
        ResetButtons();
        CocBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        CocBtn.TextColor = Color.FromArgb("#00d4ff");
        SetPage(new CocEndorsementPage());
    }

    private void ShowDocuments()
    {
        ResetButtons();
        DocumentsBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        DocumentsBtn.TextColor = Color.FromArgb("#00d4ff");
        SetPage(new DocumentsPage());
    }

    private void ShowMedicine()
    {
        ResetButtons();
        MedicineBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        MedicineBtn.TextColor = Color.FromArgb("#00d4ff");
        SetPage(new MedicinePage());
    }

    private void ShowOther()
    {
        ResetButtons();
        OtherBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        OtherBtn.TextColor = Color.FromArgb("#00d4ff");
        SetPage(new OtherPage());
    }

    private void ShowExperience()
    {
        ResetButtons();
        ExperienceBtn.BackgroundColor = Color.FromArgb("#1a3a52");
        ExperienceBtn.TextColor = Color.FromArgb("#00d4ff");
        SetPage(new ExperiencePage());
    }

    public void SetPage(ContentPage page)
    {
        Debug.WriteLine("====================================");
        Debug.WriteLine($"🔵 SetPage START: {page.GetType().Name}");

        if (_currentPage != null)
            Debug.WriteLine($"🟡 Previous page: {_currentPage.GetType().Name}");
        else
            Debug.WriteLine("🟡 Previous page: NULL");

        _currentPage = page;

        // --- текущий контент контейнера ---
        if (ContentArea.Content != null)
            Debug.WriteLine($"📦 ContentArea BEFORE: {ContentArea.Content.GetType().Name}");
        else
            Debug.WriteLine("📦 ContentArea BEFORE: NULL");

        // --- контент страницы ---
        if (page.Content != null)
        {
            Debug.WriteLine($"📄 Page.Content: {page.Content.GetType().Name}");

            if (page.Content.Parent != null)
                Debug.WriteLine($"⚠️ Page.Content уже имеет Parent: {page.Content.Parent.GetType().Name}");
            else
                Debug.WriteLine("✅ Page.Content без Parent");
        }
        else
        {
            Debug.WriteLine("❌ Page.Content = NULL");
        }

        // --- ОТРЫВАЕМ ---
        var view = page.Content;
        page.Content = null;

        Debug.WriteLine("✂️ Отцепили Content от страницы");

        // --- чистим контейнер ---
        ContentArea.Content = null;
        Debug.WriteLine("🧹 ContentArea очищен");

        // --- вставляем ---
        ContentArea.Content = view;
        Debug.WriteLine($"📥 Вставили: {view?.GetType().Name}");

        // --- проверка после ---
        if (view?.Parent != null)
            Debug.WriteLine($"🔗 Новый Parent: {view.Parent.GetType().Name}");
        else
            Debug.WriteLine("❌ У view нет Parent после вставки");

        // --- спец логика ---
        if (page is CertificatePage certPage)
        {
            Debug.WriteLine("📜 Detected CertificatePage → loading data...");
            _ = certPage.LoadCertificatesPublic();
        }

        Debug.WriteLine($"✅ SetPage END: {page.GetType().Name}");
        Debug.WriteLine("====================================");
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