using LOFit.DataServices.Admin;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;
using LOFit.Tools;
using Microsoft.Maui.Controls;

namespace LOFit.Pages.Admin.VerifyLists;

public partial class VerifyAppUsersPage : ContentPage
{
    private IAdminRestService _dataService;
    private List<Button> _buttons;
    private int _type;

    public VerifyAppUsersPage(IAdminRestService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3 };
        ListLoad(1);
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

        string wynik = await _dataService.UnblockAccount(property, _type);
    }
    async void OnNoButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var property = (int)button.CommandParameter;

        string wynik = await _dataService.BlockAccount(property, _type);
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var property = (int)button.CommandParameter;

        if (await _dataService.DeleteAccount(property, _type) == "OK")
            ListLoad(_type);
    }
    #endregion

    #region List
    async void ListLoad(int type)
    {
        if (type == 0) collectionView.ItemsSource = await _dataService.GetAppUsersAdmins();
        if (type == 1) collectionView.ItemsSource = await _dataService.GetAppUsers();
        if (type == 2) collectionView.ItemsSource = await _dataService.GetAppUsersCoachs();

        _type = await Task.Run(() => type);

        DataTools.ButtonNotClicked(_buttons);
        DataTools.ButtonClicked(_buttons[type]);
    }
    async void OnUserClicked(object sender, SelectionChangedEventArgs e)
    {
        UserModel coach = e.CurrentSelection.FirstOrDefault() as UserModel;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(UserModel), coach }
        };

        // await Shell.Current.GoToAsync(nameof(UserPage), navigationParameter);
    }
    #endregion
}