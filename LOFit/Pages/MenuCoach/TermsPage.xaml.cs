using LOFit.DataServices.Coach;
using LOFit.DataServices.Term;
using LOFit.DataServices.User;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.MenuCoach;
using LOFit.Pages.Coachs;
using LOFit.Pages.Meals;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Pages.Workouts;
using LOFit.Tools;

namespace LOFit.Pages.MenuCoach;

[QueryProperty(nameof(Client), "Client")]
[QueryProperty(nameof(DateCalendar), "DateCalendar")]
[QueryProperty(nameof(TermTimeOd), "TermTimeOd")]
[QueryProperty(nameof(TermTimeDo), "TermTimeDo")]
public partial class TermsPage : ContentPage
{

    private readonly ITermRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;
    private List<Button> _buttons;

    #region Binding prop

    UserModel _client;
    public UserModel Client
    {
        get { return _client; }
        set
        {
            _client = value;

            if (_client.Id != 0)
            {
                GridAddTerm.IsVisible = true;
                ButtonClient.Text = _client.Wizytowka();
            }

            OnPropertyChanged();
        }
    }

    DateTime _data;
    public DateTime DateCalendar
    {
        get { return _data; }
        set
        {
            if (value.Year == 1900) return;
            _data = value;
            LabelWeekDay.Text = TermTools.ReturnWeekDay(_data);

            Singleton.Instance.DateToShow = _data;

            OnPropertyChanged();
            EntryData();
        }
    }

    TimeSpan _termTimeOd;
    public TimeSpan TermTimeOd
    {
        get { return _termTimeOd; }
        set
        {
            _termTimeOd = value;

            int minutes = 60;

            if (Singleton.Instance.CoachMinutes != null)
                minutes = (int)Singleton.Instance.CoachMinutes;

            TermTimeDo = value.Add(new TimeSpan(0, minutes, 0));

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

    public TermsPage(ITermRestService dataService, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3, Button4 };
        DateCalendar = DateTime.Today;
        Singleton.Instance.IdUsera = 0;

        #region Swipe left
        SwipeGestureRecognizer swipeGestureLeft = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Left
        };

        swipeGestureLeft.Swiped += (s, e) => OnLeftSwiped();

        Content.GestureRecognizers.Add(swipeGestureLeft);
        #endregion
    }

    #region Swipe
    async void OnLeftSwiped()
    {
        await Shell.Current.GoToAsync(nameof(PlansPage));
    }
    #endregion

    #region Menu buttons
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

    #region Date buttons
    void OnButtonDateClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var value = Int32.Parse((string)button.CommandParameter);

        DateCalendar = DateCalendar.AddDays(value);
    }
    void EntryData()
    {
        _buttons[0].Text = DateCalendar.AddDays(-2).ToString("dd");
        _buttons[1].Text = DateCalendar.AddDays(-1).ToString("dd");
        _buttons[2].Text = DateCalendar.AddDays(1).ToString("dd");
        _buttons[3].Text = DateCalendar.AddDays(2).ToString("dd");

        ListLoad();
    }
    #endregion

    #region List
    async void ListLoad()
    {
        collectionView.ItemsSource = await ListModelTools.ReturnTermList((await _dataService.GetByDay(DateCalendar))?.OrderBy(x => x.MinOd()).ToList(), _dataServiceUser, _dataServiceCoach);
    }
    async void OnTermClicked(object sender, SelectionChangedEventArgs e)
    {
            TermListModel listModel = e.CurrentSelection.FirstOrDefault() as TermListModel;
            TermModel term = listModel.Term;

            Singleton.Instance.IdUsera = term.Id_usera;

            await Shell.Current.GoToAsync(nameof(MeasurementPage));
        
    }
    async void OnDeleteTermClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            bool result = await DisplayAlert($"Usuñ termin", "Czy aby na pewno?", "Tak", "Nie");

            if(result)
            {
                var button = (Button)sender;
                var id = Int32.Parse(button.CommandParameter.ToString());

                await _dataService.Delete(id);

                Dispatcher.Dispatch(() =>
                {
                    DateCalendar = DateCalendar;
                });
            }
        }
    }
    #endregion

    #region Bottom menu
    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (GridAddTerm.IsVisible)
        {
            if (Client.Id == 0)
            {
                await DisplayAlert("Dodaj termin", "Wska¿ klienta.", "Ok");
            }

            List<TermModel> dayList = ((List<TermListModel>)collectionView.ItemsSource).Select(x=>x.Term).ToList();

            bool wynik = TermTools.ChechNewTerm(dayList, TermTimeOd, TermTimeDo);

            if (wynik)
            {
                TermModel model = new TermModel()
                {
                    Id_usera = Client.Id,
                    Termin_od = new DateTime(DateCalendar.Year, DateCalendar.Month, DateCalendar.Day, TermTimeOd.Hours, TermTimeOd.Minutes, 0),
                    Termin_do = new DateTime(DateCalendar.Year, DateCalendar.Month, DateCalendar.Day, TermTimeDo.Hours, TermTimeDo.Minutes, 0)
                };

                var answer = await _dataService.Add(model);
                ListLoad();
                GridAddTerm.IsVisible = false;
            }
            else
            {
                await DisplayAlert("Dodaj termin", "Nowy termin nachodzi na istniej¹ce.", "Ok");
            }
        }
        else
        {
            GridAddTerm.IsVisible = true;
        }
    }
    async void OnSelectClientButton(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
                { "Client", Client },
                { "DateCalendar", DateCalendar },
                { "TermTimeOd", TermTimeOd },
                { "TermTimeDo", TermTimeDo }
            };

        await Shell.Current.GoToAsync(nameof(ClientListPage), navigationParameter);
    }
    void OnEndButtonClicked(object sender, EventArgs e)
    {
        GridAddTerm.IsVisible = false;

    }
    async void OnSearchButtonClicked(object sender, EventArgs e)
    {

    }
    async void OnBottomMenuClicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        var parameter = (string)button.CommandParameter;

        if (parameter == "meals")
        {
            await Shell.Current.GoToAsync(nameof(MealsPage));
        }
        else if (parameter == "measure")
        {
            await Shell.Current.GoToAsync(nameof(MeasurementPage));
        }
        else if (parameter == "workout")
        {
            await Shell.Current.GoToAsync(nameof(WorkoutsPage));
        }
        else if (parameter == "coachs")
        {
            await Shell.Current.GoToAsync(nameof(CoachsPage));
        }
        else if (parameter == "coachsProfile")
        {
            OnProfileClicked(sender, e);
        }
        else if (parameter == "userProfile")
        {
            UserModel user = await _dataServiceUser.GetOne(Singleton.Instance.IdUsera);

            var navigationParameter = new Dictionary<string, object>
                {
                    { nameof(UserModel), user}
                };

            await Shell.Current.GoToAsync(nameof(ProfilePage), navigationParameter);
        }
        else if (parameter == "connections")
        {
            if (Singleton.Instance.Type == TypKonta.Trener)
                await Shell.Current.GoToAsync(nameof(ConnectionsPage));
        }
        else if (parameter == "plans")
        {
            if (Singleton.Instance.Type == TypKonta.Trener)
                await Shell.Current.GoToAsync(nameof(PlansPage));
        }
        else if (parameter == "calendar")
        {
            if (Singleton.Instance.Type == TypKonta.Trener)
                await Shell.Current.GoToAsync(nameof(TermsPage));
        }
    }
    #endregion
}