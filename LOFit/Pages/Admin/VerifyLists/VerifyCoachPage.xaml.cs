using LOFit.DataServices.Admin;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;
using LOFit.Tools;

namespace LOFit.Pages.Admin.VerifyLists;

public partial class VerifyCoachPage : ContentPage
{
    private IAdminRestService _dataService;
    private List<Button> _buttons;
    public VerifyCoachPage(IAdminRestService dataService)
	{
		InitializeComponent();
        _dataService = dataService;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3 };
        ListLoad(0);
    }

    #region Menu buttons Admin
    async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CentralAdminPage));
    }
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

    #region Type buttons
    async void OnTypeClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var property = Int32.Parse((string)button.CommandParameter);

        ListLoad(property);
    }
    #endregion

    #region Action buttons
    async void OnOkButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var property = (int)button.CommandParameter;

        string wynik = await _dataService.SetCoach(property, 1);
    }
    async void OnNoButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var property = (int)button.CommandParameter;

        string wynik = await _dataService.SetCoach(property, 2);
    }
    #endregion

    #region List
    async void ListLoad(int type)
    {
        collectionView.ItemsSource = await _dataService.GetWgTypeCoach(type);

        DataTools.ButtonNotClicked(_buttons);
        DataTools.ButtonClicked(_buttons[type]);
    }
    async void OnCoachClicked(object sender, SelectionChangedEventArgs e)
    {
        CoachModel coach = e.CurrentSelection.FirstOrDefault() as CoachModel;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CoachModel), coach }
        };

        await Shell.Current.GoToAsync(nameof(CoachPage), navigationParameter);
    }
    #endregion
}