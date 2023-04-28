using LOFit.DataServices.Coach;
using LOFit.DataServices.User;
using LOFit.DataServices.Workout;
using LOFit.DataServices.Workouts;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.Menu;
using LOFit.Pages.Coachs;
using LOFit.Pages.Menu;
using LOFit.Resources.Styles;
using LOFit.Tools;

namespace LOFit.Pages.Workouts;

[QueryProperty(nameof(Model), "WorkoutDayModel")]
[QueryProperty(nameof(ModelWorkout), "WorkoutModel")]
[QueryProperty(nameof(ButtonClicked), "buttonClicked")]
public partial class WorkoutPage : ContentPage
{
    private readonly IWorkoutRestService _workoutDataService;
    private readonly IWorkoutsRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;
    private bool _isNew;
    private bool _isNewWorkout;

    #region Binding prop

    private bool _buttonClicked;
    public bool ButtonClicked
    {
        get { return _buttonClicked; }
        set
        {
            _buttonClicked = value;
            if (!value) OnButtonAddWorkoutClicked(new object(), new EventArgs());
            else MyListClicked();

            OnPropertyChanged();
        }
    }

    WorkoutDayModel _model;
    public WorkoutDayModel Model
    {
        get { return _model; }
        set
        {
            _model = value;

            OnPropertyChanged();
            if (Model.Id_usera == 0)
            {
                _isNew = true;
            }
            else
            {
                _isNew = false;
            }
        }
    }

    WorkoutModel _modelWorkout;
    public WorkoutModel ModelWorkout
    {
        get { return _modelWorkout; }
        set
        {
            _modelWorkout = value;

            if(_modelWorkout != null && _model != null && _model.Czas != null)
            {
                DateTime dt = (DateTime)Model.Czas;
                WorkoutingTime = dt.TimeOfDay;
            }
            if (_modelWorkout != null && _modelWorkout.Czas != null)
            {
                DateTime dt = (DateTime)_modelWorkout.Czas;
                WorkoutingTime = dt.TimeOfDay;
            }

            OnPropertyChanged();

            _isNewWorkout = _modelWorkout.Id == 0;
        }
    }

    TimeSpan _workoutingTime;
    public TimeSpan WorkoutingTime
    {
        get { return _workoutingTime; }
        set
        {
            _workoutingTime = value;

            if (ModelWorkout != null && ModelWorkout.Czas != null && ModelWorkout.Kcla != null)
            {
                DateTime czas = (DateTime)ModelWorkout.Czas;
                int minuty = czas.Hour * 60 + czas.Minute;

                int minuty2 = value.Hours * 60 + value.Minutes;

                Model.Kcla = minuty2 * ModelWorkout.Kcla / minuty;
            }

            OnPropertyChanged();
        }
    }

    TimeSpan _workoutTime;
    public TimeSpan WorkoutTime
    {
        get { return _workoutTime; }
        set
        {
            _workoutTime = value;
            OnPropertyChanged();
        }
    }
    #endregion

    public WorkoutPage(IWorkoutsRestService dataService, IWorkoutRestService workoutDataService, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
	{
		InitializeComponent();
        _dataService = dataService;
        _workoutDataService = workoutDataService;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
        BindingContext = this;

        WorkoutTime = DateTime.Now.TimeOfDay;

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) => OnRightSwiped();

        Content.GestureRecognizers.Add(swipeGestureRight);
        #endregion
    }
    #region Swipe
    async void OnRightSwiped()
    {
        Singleton.Instance.DateToShow = Model.Data_czas;
        await Shell.Current.GoToAsync(nameof(WorkoutsPage));
    }
    #endregion

    #region Menu buttons
    async void OnBackClicked(object sender, EventArgs e)
    {
        Singleton.Instance.DateToShow = Model.Data_czas;
        await Shell.Current.GoToAsync(nameof(WorkoutsPage));
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

    #region Workout buttons
    void OnButtonAddWorkoutClicked(object sender, EventArgs e)
    {
        DataTools.ButtonClicked(ButtonAddWorkout, BottomAddWorkout);
        DataTools.ButtonNotClicked(ButtonMyList, BottomMyList);

        IsNewWorkout(true);

        ModelWorkout = new WorkoutModel();
    }
    async void OnButtonMyListClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "workoutDayDate", Model.Data_czas }
        };

        await Shell.Current.GoToAsync(nameof(WorkoutsListPage), navigationParameter);
    }
    private void IsNewWorkout(bool isNew)
    {
        checkBoxGrid.IsVisible = isNew;
        EntryNazwa.IsReadOnly = !isNew;
        EntryNazwa.BackgroundColor = isNew ? MyColors.MyEntryBg : MyColors.MyBg;
    }
    private void MyListClicked()
    {
        DataTools.ButtonNotClicked(ButtonAddWorkout, BottomAddWorkout);
        DataTools.ButtonClicked(ButtonMyList, BottomMyList);

        IsNewWorkout(false);
    }
    #endregion

    #region Bottom menu
    async void OnModifyButtonClicked(object sender, EventArgs e)
    {
        Model.Trening = ModelWorkout;
        if (Model.Trening == null) return;

        string answer;

        Model.Data_czas = new DateTime(Model.Data_czas.Year, Model.Data_czas.Month, Model.Data_czas.Day, WorkoutTime.Hours, WorkoutTime.Minutes, WorkoutTime.Seconds);
        Model.Czas = new DateTime(Model.Data_czas.Year, Model.Data_czas.Month, Model.Data_czas.Day, WorkoutingTime.Hours, WorkoutingTime.Minutes, WorkoutingTime.Seconds);
        Model.Id_usera = Singleton.Instance.IdUsera;

        if (_isNewWorkout)
        {
            ModelWorkout.Kcla = Model.Kcla;
            ModelWorkout.Czas = Model.Czas;
            ModelWorkout.Opis = Model.Opis;

            if (checkBox.IsChecked) ModelWorkout.Id_konta = -1;
            else ModelWorkout.Id_konta = 0;

            ModelWorkout.Id = await _workoutDataService.Add(ModelWorkout);
        }

        Model.Id_treningu = ModelWorkout.Id;

        if (Model.Id_treningu == 0) return;

        if (_isNew)
        {
            if (Singleton.Instance.Type == TypKonta.Trener)
                Model.Id_trenera = Singleton.Instance.IdTrenera;

            answer = await _dataService.Add(Model);
        }
        else
            answer = await _dataService.Update(Model);

        if (answer == "Ok")
            await Shell.Current.GoToAsync(nameof(WorkoutsPage));
    }
    #endregion
}