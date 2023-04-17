using LOFit.Models.Accounts;
using LOFit.Pages.Admin.VerifyLists;
using LOFit.Tools;

namespace LOFit.Pages.Admin;

public partial class CentralAdminPage : ContentPage
{
    #region Info
    public string InfoCoachs { get; set; }
    public string InfoCertificate { get; set; }
    public string InfoVerifyOpinion { get; set; }
    public string InfoVerifyReport { get; set; }
    public string InfoAppUsers { get; set; }
    #endregion

    public CentralAdminPage()
	{
		InitializeComponent();
        BindingContext = this;

        InfoCoachs = "0";
        InfoCertificate = "0";
        InfoVerifyOpinion = "0";
        InfoVerifyReport = "0";
        InfoAppUsers = "";
	}

    #region Menu buttons Admin
    async void OnSettingsClicked(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync(nameof(SettingsUserPage));
    }
    async void OnLogoutClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            {nameof(LoginModel), new LoginModel(false)
                {
                    Email = Singleton.Instance.User,
                }
            }
        };

        Singleton.Logout();
        await Shell.Current.GoToAsync("Login", navigationParameter);
    }
    #endregion

    #region Center buttons
    async void OnButtonVerifyCoachClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(VerifyCoachPage));
    }
    async void OnButtonVerifyCertificateClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(VerifyCertificatePage));
    }
    async void OnButtonVerifyOpinionClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(VerifyOpinionPage));
    }
    async void OnButtonVerifyReportClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(VerifyReportPage));
    }
    async void OnButtonAppUsersClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(VerifyAppUsersPage));
    }
    #endregion

}