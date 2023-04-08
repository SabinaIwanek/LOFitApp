using LOFit.DataServices.Measurement;
using LOFit.Models;
using LOFit.Pages.Central;
using LOFit.Pages.Coachs;
using LOFit.Pages.Meals;
using LOFit.Pages.Menu;
using LOFit.Pages.Workouts;
using LOFit.Tools;
using Microsoft.Maui.ApplicationModel;

namespace LOFit.Pages.Measures;

[QueryProperty(nameof(Model), "MeasurementModel")]
public partial class MeasurementPage : ContentPage
{
    private readonly IMeasurementRestService _dataService;
    private bool _isNew;
    private List<Button> _buttons;
    private DateTime _firstDayWeek;
    private List<MeasurementModel> _weekModels;
    private bool _buttonsWorking = true;

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
            if (_firstDayWeek.Year == 1)
                return;

            _data = value;

            Singleton.Instance.DateToShow = _data;

            if (_data < _firstDayWeek || _data > _firstDayWeek.AddDays(6) || _weekModels == null)
            {
                EntryWeekButtons(_data);
            }
            else
            {
                DataTools.ButtonClicked(_data.DayOfWeek, _buttons);
            }

            OnPropertyChanged();
        }
    }
    #endregion
    public MeasurementPage(IMeasurementRestService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3, Button4, Button5, Button6, Button7 };
       
        if(Singleton.Instance.DateToShow.Year != 1)
        {
            EntryWeekButtons(Singleton.Instance.DateToShow);
            DateCalendar = Singleton.Instance.DateToShow;
        }
        else
        {
            EntryWeekButtons(DateTime.Today);
            DateCalendar = DateTime.Today;
        }

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
    async void OnChangeThemeClicked(object sender, EventArgs e)
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

        Model = new MeasurementModel(_weekModels[0]);
        DateCalendar = Model.Data_pomiaru;
    }
    async void OnButton2Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        Model = new MeasurementModel(_weekModels[1]);
        DateCalendar = Model.Data_pomiaru;
    }
    async void OnButton3Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        Model = new MeasurementModel(_weekModels[2]);
        DateCalendar = Model.Data_pomiaru;
    }
    async void OnButton4Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        Model = new MeasurementModel(_weekModels[3]);
        DateCalendar = Model.Data_pomiaru;
    }
    async void OnButton5Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        Model = new MeasurementModel(_weekModels[4]);
        DateCalendar = Model.Data_pomiaru;
    }
    async void OnButton6Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        Model = new MeasurementModel(_weekModels[5]);
        DateCalendar = Model.Data_pomiaru;
    }
    async void OnButton7Clicked(object sender, EventArgs e)
    {
        if (!_buttonsWorking) return;

        Model = new MeasurementModel(_weekModels[6]);
        DateCalendar = Model.Data_pomiaru;
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
        if (_isNew)
        {
            string answer = await _dataService.Add(Model);

            if (answer == "Ok")
                _isNew = false;
        }
        else await _dataService.Update(Model);

        // Odœwie¿anie
        Model = Model;
        EntryWeekButtons(Model.Data_pomiaru);
    }
    #endregion

    private async void EntryWeekButtons(DateTime date)
    {
        _firstDayWeek = DataTools.ReturnFirstDayWeek(date);
        _weekModels = new List<MeasurementModel>();

        _buttonsWorking = false;

        int buttonIndex = DataTools.ButtonClicked(date.DayOfWeek, _buttons);

        _weekModels = await _dataService.GetWeek(_firstDayWeek);
        Model = await Task.Run(() => new MeasurementModel(_weekModels[buttonIndex]));

        for (int i = 0; i < 7; i++)
        {
            _buttons[i].Text = _firstDayWeek.AddDays(i).ToString("dd.MM");
        }

        _buttonsWorking = true;
    }
}