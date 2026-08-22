namespace Docs_Manager.View;

public partial class SendPage : ContentView
{
    private MainPage? _mainPage;

    public SendPage(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage;
    }

    private void OnSendClicked(object sender, EventArgs e)
    {
        bool emailSelected = EmailCheckBox.IsChecked;
        bool printSelected = PrintCheckBox.IsChecked;
        bool cloudSelected = CloudCheckBox.IsChecked;
        bool usbSelected = UsbCheckBox.IsChecked;

        if (!emailSelected && !printSelected && !cloudSelected && !usbSelected)
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please select at least one delivery method", "OK");
            return;
        }

        if (emailSelected && string.IsNullOrEmpty(EmailEntry.Text))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please enter email address", "OK");
            return;
        }

        var methods = new List<string>();
        if (emailSelected) methods.Add($"Email: {EmailEntry.Text}");
        if (printSelected) methods.Add("Print");
        if (cloudSelected) methods.Add("Cloud Backup");
        if (usbSelected) methods.Add("USB Export");

        Application.Current.MainPage.DisplayAlert(
            "Success",
            $"Documents will be sent via:\n{string.Join("\n", methods)}",
            "OK");
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        EmailCheckBox.IsChecked = false;
        PrintCheckBox.IsChecked = false;
        CloudCheckBox.IsChecked = false;
        UsbCheckBox.IsChecked = false;
        EmailEntry.Text = string.Empty;
        SubjectEntry.Text = string.Empty;
        EmailSection.IsVisible = false;
    }
}
