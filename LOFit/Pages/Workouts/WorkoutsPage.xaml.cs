using LOFit.DataServices.Coach;
using LOFit.DataServices.Measurement;
using LOFit.DataServices.User;
using LOFit.DataServices.Workouts;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.Menu;
using LOFit.Pages.Coachs;
using LOFit.Pages.Meals;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Tools;

namespace LOFit.Pages.Workouts;

public partial class WorkoutsPage : ContentPage
{
    private readonly IWorkoutsRestService _dataService;
    private readonly IMeasurementRestService _dataServiceMeasurement;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;
    private List<Button> _buttons;

    #region Binding prop
    DateTime _data;
    public DateTime DateCalendar
    {
        get { return _data; }
        set
        {
            if (value.Year == 1900) return;

            _data = value;

            Singleton.Instance.DateToShow = _data;
            
            OnPropertyChanged();
            EntryDate();
            ListLoad();
        }
    }
    #endregion

    public WorkoutsPage(IWorkoutsRestService dataService, IMeasurementRestService dataServiceMeasurement, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceMeasurement = dataServiceMeasurement;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3, Button4 };

        DateCalendar = Singleton.Instance.DateToShow;

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) => OnRightSwiped();

        Content.GestureRecognizers.Add(swipeGestureRight);
        #endregion

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
    async void OnRightSwiped()
    {
        await Shell.Current.GoToAsync(nameof(MeasurementPage));
    }
    async void OnLeftSwiped()
    {
        await Shell.Current.GoToAsync(nameof(CoachsPage));
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
    void EntryDate()
    {
        _buttons[0].Text = DateCalendar.AddDays(-2).ToString("dd");
        _buttons[1].Text = DateCalendar.AddDays(-1).ToString("dd");
        _buttons[2].Text = DateCalendar.AddDays(1).ToString("dd");
        _buttons[3].Text = DateCalendar.AddDays(2).ToString("dd");
    }
    #endregion

    #region List
    async void ListLoad()
    {
        var list = await _dataService.GetUserList(DateCalendar, Singleton.Instance.IdUsera);

        Dispatcher.Dispatch(() =>
        {
            collectionView.ItemsSource = ListModelTools.ReturnWorkoutDayList(list.Where(x => x.Id_trenera == null).ToList());
            collectionViewCoach.ItemsSource = ListModelTools.ReturnWorkoutDayList(list.Where(x => x.Id_trenera != null).ToList());

            if (!list.Where(x => x.Id_trenera != null).Any())
            {
                Header1.IsVisible = false;
                Header2.IsVisible = false;
            }
            else
            {
                Header1.IsVisible = true;
                Header2.IsVisible = true;
            }
        });
    }
    async void OnWorkoutClicked(object sender, SelectionChangedEventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
        {
            WorkoutDayListModel listModel = e.CurrentSelection.FirstOrDefault() as WorkoutDayListModel;
            WorkoutDayModel dzienTreningu = listModel.WorkoutDay;

            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(WorkoutDayModel), dzienTreningu },
                { nameof(WorkoutModel), dzienTreningu.Trening},
                { "buttonClicked", true }
            };

            await Shell.Current.GoToAsync(nameof(WorkoutPage), navigationParameter);
        }
    }
    async void OnDeleteWorkoutClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
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

    async void OnCoachWorkoutClicked(object sender, SelectionChangedEventArgs e)
    {
        WorkoutDayListModel listModel = e.CurrentSelection.FirstOrDefault() as WorkoutDayListModel;
        WorkoutDayModel dzienTreningu = listModel.WorkoutDay;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(WorkoutDayModel), dzienTreningu },
            { nameof(WorkoutModel), dzienTreningu.Trening},
            { "buttonClicked", true }
        };

        await Shell.Current.GoToAsync(nameof(WorkoutPage), navigationParameter);
    }
    async void OnDeleteCoachWorkoutClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Trener)
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
    #endregion

    #region Bottom Button

    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(WorkoutDayModel), new WorkoutDayModel(){ Data_czas = DateCalendar} },
            { nameof(WorkoutModel), new WorkoutModel() },
            { "buttonClicked", false }
        };

        await Shell.Current.GoToAsync(nameof(WorkoutPage), navigationParameter);
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
    }
    #endregion
}