using LOFit.DataServices.Coach;
using LOFit.DataServices.Measurement;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.Menu;
using LOFit.Pages.Coachs;
using LOFit.Pages.Meals;
using LOFit.Pages.Menu;
using LOFit.Pages.Workouts;
using LOFit.Tools;

namespace LOFit.Pages.Measures;

[QueryProperty(nameof(Model), "MeasurementModel")]
public partial class MeasurementPage : ContentPage
{
    private readonly IMeasurementRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private List<Button> _buttons;
    private bool _isNew;

    #region Binding prop
    MeasurementModel _model;
    public MeasurementModel Model
    {
        get { return _model; }
        set
        {
            _model = value;
            _isNew = Model.Id_usera == 0;

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

            Singleton.Instance.DateToShow = _data;

            OnPropertyChanged();
            EntryData();
        }
    }
    #endregion
    public MeasurementPage(IMeasurementRestService dataService, ICoachRestService dataServiceCoach)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3, Button4 };

        if (Singleton.Instance.DateToShow.Year != 1) DateCalendar = Singleton.Instance.DateToShow;
        else DateCalendar = DateTime.Today;

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
        await Shell.Current.GoToAsync(nameof(MealsPage));
    }
    async void OnLeftSwiped()
    {
        await Shell.Current.GoToAsync(nameof(WorkoutsPage));
    }
    #endregion

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
        if(Singleton.Instance.Type == TypKonta.Uzytkownik)
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

    #region Date buttons
    void OnButtonDateClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var value = Int32.Parse((string)button.CommandParameter);

        DateCalendar = DateCalendar.AddDays(value);
    }
    async void EntryData()
    {
        _buttons[0].Text = DateCalendar.AddDays(-2).ToString("dd");
        _buttons[1].Text = DateCalendar.AddDays(-1).ToString("dd");
        _buttons[2].Text = DateCalendar.AddDays(1).ToString("dd");
        _buttons[3].Text = DateCalendar.AddDays(2).ToString("dd");

        Model = await _dataService.Get(DateCalendar);
    }
    #endregion

    #region Plus / Minus
    void OnButtonMinusClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var propertyName = (string)button.CommandParameter;

        var propertyInfo = Model.GetType().GetProperty(propertyName);
        var propertyValue = (decimal?)propertyInfo.GetValue(Model);

        if (propertyValue == null) propertyValue = 0;
        else propertyValue--;

        propertyInfo.SetValue(Model, propertyValue);
    }
    void OnButtonPlusClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var propertyName = (string)button.CommandParameter;

        var propertyInfo = Model.GetType().GetProperty(propertyName);
        var propertyValue = (decimal?)propertyInfo.GetValue(Model);

        if (propertyValue == null) propertyValue = 1;
        else propertyValue++;

        propertyInfo.SetValue(Model, propertyValue);
    }
    #endregion

    #region Bottom menu
    async void OnModifyButtonClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
        {
            if (_isNew)
            {
                string answer = await _dataService.Add(Model);

                if (answer == "Ok")
                    _isNew = false;
            }
            else await _dataService.Update(Model);

            Model = Model;
        }
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