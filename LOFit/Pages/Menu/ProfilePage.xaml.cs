using LOFit.DataServices.Coach;
using LOFit.Tools;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;
using LOFit.Pages.Measures;
using LOFit.DataServices.User;
using LOFit.Resources.Styles;
using LOFit.Pages.Meals;
using LOFit.Pages.Workouts;

namespace LOFit.Pages.Menu;

[QueryProperty(nameof(UserM), "UserModel")]
public partial class ProfilePage : ContentPage
{
    private readonly IUserRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;

    #region Binding prop

    UserModel _model;
    public UserModel UserM
    {
        get { return _model; }
        set
        {
            _model = value;
            if (_model != null)
            {
                Wizytowka = _model.Wizytowka();
                DataUr = _model.DataUr();
                PlecString = _model.PlecString();
                Telefon = _model.Telefon();

                OnPropertyChanged();
            }
        }
    }

    string _wizytowka;
    public string Wizytowka
    {
        get { return _wizytowka; }
        set { _wizytowka = value; OnPropertyChanged(); }
    }

    string _dataUr;
    public string DataUr
    {
        get { return _dataUr; }
        set { _dataUr = value; OnPropertyChanged(); }
    }

    string _plecString;
    public string PlecString
    {
        get { return _plecString; }
        set { _plecString = value; OnPropertyChanged(); }
    }

    string _telefon;
    public string Telefon
    {
        get { return _telefon; }
        set { _telefon = value; OnPropertyChanged(); }
    }
    #endregion
    public ProfilePage(IUserRestService dataService, ICoachRestService dataServiceCoach)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;

        BottomMenuLoad();

        BindingContext = this;

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) => OnRightSwiped();

        Content.GestureRecognizers.Add(swipeGestureRight);
        #endregion
    }

    #region Swiped
    async void OnRightSwiped()
    {
        await Shell.Current.GoToAsync(nameof(WorkoutsPage));
    }

    #endregion
    #region Menu buttons
    async void OnBackClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
        {
            await Shell.Current.GoToAsync(nameof(MeasurementPage));
        }
        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            await Shell.Current.GoToAsync(nameof(ConnectionsPage));
        }
    }
    async void OnProfileClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
        {
            UserModel model = await _dataService.GetOne(-1);

            var navigationParameter = new Dictionary<string, object>
                {
                    { nameof(UserModel), model}
                };

            await Shell.Current.GoToAsync(nameof(ProfilePage), navigationParameter);
        }

        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            CoachModel model = await _dataServiceCoach.GetOne(-1);

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

    #region User Info
    async void OnEditClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
        {
            var navigationParameter = new Dictionary<string, object>
                {
                    { nameof(CoachModel), new CoachModel()},
                    { nameof(UserModel), UserM}
                };

            await Shell.Current.GoToAsync(nameof(EditProfilePage), navigationParameter);
        }
    }
    #endregion

    #region Bottom menu
    void BottomMenuLoad()
    {
        bool isMyCoach = Singleton.Instance.Type == TypKonta.Trener && Singleton.Instance.IdUsera != 0;

        CoachBottomMenu.IsVisible = isMyCoach;
        EditImageButton.IsVisible = Singleton.Instance.Type == TypKonta.Uzytkownik;
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
            UserModel user = await _dataService.GetOne(Singleton.Instance.IdUsera);

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
        }
        else if (parameter == "calendar")
        {
        }
    }
    #endregion
}