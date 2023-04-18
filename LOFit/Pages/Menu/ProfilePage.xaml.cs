using LOFit.DataServices.Coach;
using LOFit.Tools;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;

namespace LOFit.Pages.Menu;

public partial class ProfilePage : ContentPage
{
    private readonly ICoachRestService _dataServiceCoach;
    public ProfilePage(ICoachRestService dataServiceCoach)
	{
		InitializeComponent();
        _dataServiceCoach = dataServiceCoach;
    }
    #region Menu buttons
    async void OnCoachsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CoachsPage));
    }
    void OnChangeThemeClicked(object sender, EventArgs e)
    {
        if (App.Current.UserAppTheme == AppTheme.Dark)
        {
            App.Current.UserAppTheme = AppTheme.Light;
        }
        else
        {
            App.Current.UserAppTheme = AppTheme.Dark;
        }
    }
    async void OnProfileClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
            await Shell.Current.GoToAsync(nameof(ProfilePage));

        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            CoachModel model = await _dataServiceCoach.GetOne(-1);
            Singleton.Instance.IdTrenera = model.Id;

            var navigationParameter = new Dictionary<string, object>
                {
                    { nameof(CoachModel), model}
                };

            await Shell.Current.GoToAsync(nameof(CoachPage), navigationParameter);
        }
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
}