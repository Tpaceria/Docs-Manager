namespace Docs_Manager.View;

public partial class FilesPage : ContentPage
{
    public FilesPage()
    {
        InitializeComponent();
    }

    async void OnCertificatesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CertificatePage());
    }

    async void OnCocClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CocEndorsementPage());
    }

    async void OnDocumentsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DocumentsPage());
    }

    async void OnMedicineClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MedicinePage());
    }

    async void OnOtherClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OtherPage());
    }
}