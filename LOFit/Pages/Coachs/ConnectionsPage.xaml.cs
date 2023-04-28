using LOFit.DataServices.Coach;
using LOFit.DataServices.Connection;
using LOFit.DataServices.User;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.ProfileMenu;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Tools;

namespace LOFit.Pages.Coachs;

public partial class ConnectionsPage : ContentPage
{
    private readonly IConnectionRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;
    private List<Button> _buttons;
    private List<Grid> _grids;
    private int _type;

    public ConnectionsPage(IConnectionRestService dataService, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
        _buttons = new List<Button>() { NewButton, ActiveButton, HistoryButton };
        _grids = new List<Grid>() { BottomNewButton, BottomActiveButton, BottomHistoryButton };
        ListLoad(0);

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) => OnSwipeRight();

        Content.GestureRecognizers.Add(swipeGestureRight);
        #endregion
    }

    #region Swiped
    async void OnSwipeRight()
    {
        CoachModel model = await _dataServiceCoach.GetOne(-1);

        var navigationParameter = new Dictionary<string, object>
                {
                    { nameof(CoachModel), model}
                };

        await Shell.Current.GoToAsync(nameof(CoachPage), navigationParameter);
    }
    #endregion

    #region Menu buttons
    async void OnCoachsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CoachsPage));
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

    #region List
    async void ListLoad(int type)
    {
        var list = await _dataService.GetCoachList(-1);

        collectionViewNew.IsVisible = false;
        collectionViewActual.IsVisible = false;
        collectionViewHistory.IsVisible = false;

        Dispatcher.Dispatch(async () =>
        {
            if (type == 0)
            {
                collectionViewNew.ItemsSource = await ListModelTools.ReturnConnectionList(list.Where(x => x.Zatwierdzone == type).ToList(), _dataServiceUser, _dataServiceCoach);
                collectionViewNew.IsVisible = true;
            }
            if (type == 1)
            {
                collectionViewActual.ItemsSource = await ListModelTools.ReturnConnectionList(list.Where(x => x.Zatwierdzone == type).ToList(), _dataServiceUser, _dataServiceCoach);
                collectionViewActual.IsVisible = true;
            }
            if (type == 3)
            {
                collectionViewHistory.ItemsSource = await ListModelTools.ReturnConnectionList(list, _dataServiceUser, _dataServiceCoach);
                collectionViewHistory.IsVisible = true;
            }
        });
    }

    async void OnConnectionClicked(object sender, SelectionChangedEventArgs e)
    {
        if (_type == 1)
        {
            var model = e.CurrentSelection.FirstOrDefault() as ConnectionListModel;
            Singleton.Instance.IdUsera = model.Connection.Id_usera;

            await Shell.Current.GoToAsync(nameof(MeasurementPage));
        }
    }
    void OnListTypeClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        _type = Int32.Parse((string)button.CommandParameter);

        ListLoad(_type);

        int index = _type;
        if (_type == 3) index = 2;

        DataTools.ButtonNotClicked(_buttons, _grids);
        DataTools.ButtonClicked(_buttons[index], _grids[index]);

    }
    async void OnDecideOkClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var id = (int)button.CommandParameter;

        await _dataService.UpdateStateOk(id);
        ListLoad(_type);
    }
    async void OnDecideNoClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var id = (int)button.CommandParameter;

        await _dataService.UpdateStateNo(id);
        ListLoad(_type);
    }
    #endregion
}