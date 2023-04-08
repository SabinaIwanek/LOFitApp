using LOFit.DataServices.Meals;
using LOFit.Models;
using LOFit.Pages.Coachs;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Tools;

namespace LOFit.Pages.Meals;

public partial class MealsPage : ContentPage
{
    private readonly IMealRestService _dataService;
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
                EntryWeekDates(_data);
            }
            else
            {
                int buttonIndex = DataTools.ButtonClicked(_data.DayOfWeek, _buttons);
                _lastButtonClicked = _buttons[buttonIndex];
            }

            ListLoad(_data);

            OnPropertyChanged();
        }
    }
    #endregion

    public MealsPage(IMealRestService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3, Button4, Button5, Button6, Button7 };

        if(Singleton.Instance.DateToShow.Year != 1)
        {
            EntryWeekDates(Singleton.Instance.DateToShow);
            DateCalendar = Singleton.Instance.DateToShow;
        }

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

    #region List
    async void ListLoad(DateTime date)
    {
        collectionView.ItemsSource = await _dataService.GetUserList(date);
    }
    async void OnMealClicked(object sender, SelectionChangedEventArgs e)
    {
        MealModel meal = e.CurrentSelection.FirstOrDefault() as MealModel;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(MealModel), meal },
            { nameof(ProductModel), meal.Produkt},
            { "buttonClicked", 2 }
        };

        await Shell.Current.GoToAsync(nameof(MealPage), navigationParameter);
    }
    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(MealModel), new MealModel(){ Data_czas = DateCalendar} },
            { nameof(ProductModel), new ProductModel() },
            { "buttonClicked", 1 }
        };

        await Shell.Current.GoToAsync(nameof(MealPage), navigationParameter);
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