using LOFit.Models;
using LOFit.Tools;

namespace LOFit.Pages.Central;

public partial class CentralCoachPage : ContentPage
{
	public CentralCoachPage()
	{
		InitializeComponent();
    }
    async void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            {nameof(LoginModel), new LoginModel(false){Email = Singleton.Instance.User } }
        };

        Singleton.Logout();
        await Shell.Current.GoToAsync("Login", navigationParameter);
    }
    async void OnProfileButtonClicked(object sender, EventArgs e)
    {

    }
    async void OnClientButtonClicked(object sender, EventArgs e)
    {

    }
    async void OnCalendarButtonClicked(object sender, EventArgs e)
    {

    }
    async void OnCertificatesButtonClicked(object sender, EventArgs e)
    {

    }
}