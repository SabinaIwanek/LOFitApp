using LOFit.DataServices.Coach;
using LOFit.DataServices.Connection;
using LOFit.DataServices.User;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.ProfileMenu;
using LOFit.Pages.Coachs;
using LOFit.Pages.Menu;
using LOFit.Tools;

namespace LOFit.Pages.MenuCoach;

[QueryProperty(nameof(Client), "Client")]
[QueryProperty(nameof(DateCalendar), "DateCalendar")]
[QueryProperty(nameof(TermTimeOd), "TermTimeOd")]
[QueryProperty(nameof(TermTimeDo), "TermTimeDo")]
public partial class ClientListPage : ContentPage
{
    private readonly IConnectionRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;

    #region Binding prop

    UserModel _client;
    public UserModel Client
    {
        get { return _client; }
        set
        {
            _client = value;
            OnPropertyChanged();
        }
    }

    DateTime _data;
    public DateTime DateCalendar
    {
        get { return _data; }
        set
        {
            _data = value;
            OnPropertyChanged();
        }
    }

    TimeSpan _termTimeOd;
    public TimeSpan TermTimeOd
    {
        get { return _termTimeOd; }
        set
        {
            _termTimeOd = value;
            OnPropertyChanged();
        }
    }

    TimeSpan _termTimeDo;
    public TimeSpan TermTimeDo
    {
        get { return _termTimeDo; }
        set
        {
            _termTimeDo = value;
            OnPropertyChanged();
        }
    }
    #endregion
    public ClientListPage(IConnectionRestService dataService, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;

        ListLoad();
    }

    #region Menu buttons
    async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TermsPage));
    }
    async void OnProfileClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
        {
            UserModel model = await _dataServiceUser.GetOne(-1);

            var navigationParameter = new Dictionary<string, object>
                {
                    { nameof(UserModel), model}
                };

            await Shell.Current.GoToAsync(nameof(ProfilePage), navigationParameter);
        }

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
    async void ListLoad()
    {
        var list = await _dataService.GetCoachList(-1);

        Dispatcher.Dispatch(async () =>
        {
            collectionViewActual.ItemsSource = await ListModelTools.ReturnConnectionList(list.Where(x => x.Zatwierdzone == 1).ToList(), _dataServiceUser, _dataServiceCoach);
        });
    }

    async void OnConnectionClicked(object sender, SelectionChangedEventArgs e)
    {

        var model = e.CurrentSelection.FirstOrDefault() as ConnectionListModel;
        UserModel userModel = await _dataServiceUser.GetOne(model.Connection.Id_usera);

        var navigationParameter = new Dictionary<string, object>
            {
                { "Client", userModel },
                { "DateCalendar", DateCalendar },
                { "TermTimeOd", TermTimeOd },
                { "TermTimeDo", TermTimeDo }
            };

        await Shell.Current.GoToAsync(nameof(TermsPage), navigationParameter);
    }
    #endregion
}