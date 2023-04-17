using LOFit.DataServices.Measurement;
using LOFit.DataServices.Workouts;
using LOFit.Models.Accounts;
using LOFit.Models.Menu;
using LOFit.Pages.Coachs;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Tools;

namespace LOFit.Pages.Workouts;

public partial class WorkoutsPage : ContentPage
{
    private readonly IWorkoutsRestService _dataService;
    private readonly IMeasurementRestService _dataServiceMeasurement;
    private List<Button> _buttons;
    private Button _lastButtonClicked;
    private DateTime _firstDayWeek;
    private List<DateTime> _weekDates;
    private bool _buttonsWorking = true;

    #region Binding prop
    DateTime _data;
    public DateTime DateCalendar
    {
        get { return _data; }
        set
        {
            if (_firstDayWeek.Year == 1 || value == _data)
                return;

            _data = value;

            Singleton.Instance.DateToShow = _data;

            if (_data < _firstDayWeek || _data > _firstDayWeek.AddDays(6))
            {
                OnDatePickerClicked(_data);
                EntryWeekDates(_data);
            }
            else
            {
                int buttonIndex = DataTools.ButtonClicked(_data.DayOfWeek, _buttons);
                _lastButtonClicked = _buttons[buttonIndex];
            }

            ListLoad(value);

            OnPropertyChanged();
        }
    }
    #endregion

    public WorkoutsPage(IWorkoutsRestService dataService, IMeasurementRestService dataServiceMeasurement)
	{
		InitializeComponent();
        _dataService = dataService;
        _dataServiceMeasurement = dataServiceMeasurement;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3, Button4, Button5, Button6, Button7 };

        EntryWeekDates(Singleton.Instance.DateToShow);
        DateCalendar = Singleton.Instance.DateToShow;

        #region Swipe right
        SwipeGestureRecognizer swipeGestureLeft = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureLeft.Swiped += (s, e) => OnRightSwiped();

        Content.GestureRecognizers.Add(swipeGestureLeft);
        #endregion
    }

    #region Swipe
    async void OnRightSwiped()
    {
        await Shell.Current.GoToAsync(nameof(MeasurementPage));
    }
    #endregion

    #region Menu buttons
    async void OnCoachsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CoachsPage));
    }
    async void OnProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }
    async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsUserPage));
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

    #region Week buttons
    async void OnButton1Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        DateCalendar = _weekDates[0];

        DataTools.ButtonSwitch(Button1, _lastButtonClicked);
        _lastButtonClicked = Button1;
    }
    async void OnButton2Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        DateCalendar = _weekDates[1];

        DataTools.ButtonSwitch(Button2, _lastButtonClicked);
        _lastButtonClicked = Button2;
    }
    async void OnButton3Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        DateCalendar = _weekDates[2];

        DataTools.ButtonSwitch(Button3, _lastButtonClicked);
        _lastButtonClicked = Button3;
    }
    async void OnButton4Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        DateCalendar = _weekDates[3];

        DataTools.ButtonSwitch(Button4, _lastButtonClicked);
        _lastButtonClicked = Button4;
    }
    async void OnButton5Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        DateCalendar = _weekDates[4];

        DataTools.ButtonSwitch(Button5, _lastButtonClicked);
        _lastButtonClicked = Button5;
    }
    async void OnButton6Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        DateCalendar = _weekDates[5];

        DataTools.ButtonSwitch(Button6, _lastButtonClicked);
        _lastButtonClicked = Button6;
    }
    async void OnButton7Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        DateCalendar = _weekDates[6];

        DataTools.ButtonSwitch(Button7, _lastButtonClicked);
        _lastButtonClicked = Button7;
    }
    #endregion

    #region Calendar
    async void OnDatePickerClicked(DateTime data)
    {
        DataTools.ButtonNotClicked(_lastButtonClicked);
    }
    #endregion

    #region List
    async void ListLoad(DateTime date)
    {
        collectionView.ItemsSource = await _dataService.GetUserList(date, Singleton.Instance.IdUsera);
    }
    async void OnWorkoutClicked(object sender, SelectionChangedEventArgs e)
    {
        WorkoutDayModel dzienTreningu = e.CurrentSelection.FirstOrDefault() as WorkoutDayModel;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(WorkoutDayModel), dzienTreningu },
            { nameof(WorkoutModel), dzienTreningu.Trening},
            { "buttonClicked", true }
        };

        await Shell.Current.GoToAsync(nameof(WorkoutPage), navigationParameter);
    }
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
    #endregion
    private async void EntryWeekDates(DateTime date)
    {
        _firstDayWeek = DataTools.ReturnFirstDayWeek(date);
        _weekDates = new List<DateTime>();

        _buttonsWorking = false;

        int buttonIndex = DataTools.ButtonClicked(date.DayOfWeek, _buttons);
        _lastButtonClicked = _buttons[buttonIndex];

        for (int i = 0; i < 7; i++)
        {
            _weekDates.Add(_firstDayWeek.AddDays(i));
            _buttons[i].Text = _weekDates.Last().ToString("dd.MM");
        }

        _buttonsWorking = true;
    }
}