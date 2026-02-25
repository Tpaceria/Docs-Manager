using Docs_Manager.Data;
using Docs_Manager.Models;   

namespace Docs_Manager.View;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

private async void OnItemTapped(object? sender, EventArgs e)
    {
        if (sender is TapGestureRecognizer tap &&
            tap.CommandParameter is Certificate cert)
        {
            await Navigation.PushAsync(new EditCertificatePage(cert));
        }
    }
}
