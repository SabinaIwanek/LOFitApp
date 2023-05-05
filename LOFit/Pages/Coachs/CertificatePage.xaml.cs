using LOFit.DataServices.Certificate;
using LOFit.DataServices.Coach;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.ProfileMenu;
using LOFit.Pages.Menu;
using LOFit.Tools;

namespace LOFit.Pages.Coachs;

[QueryProperty(nameof(ModelC), "CertificateModel")]
public partial class CertificatePage : ContentPage
{
    private readonly ICertificateRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;

    #region Binding prop
    CertificateModel _model;
    public CertificateModel ModelC
    {
        get { return _model; }
        set
        {
            _model = value;
            DateCalendar = DateTime.Today;
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

            if (ModelC != null)
                ModelC.Data_certyfikatu = _data;
            
            OnPropertyChanged();
        }
    }
    #endregion
    public CertificatePage(ICertificateRestService dataService, ICoachRestService dataServiceCoach)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
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

    #region Swipe
    async void OnRightSwiped()
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

    #region Bottom menu
    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if(ModelC.Nazwa == "" || ModelC.Organizacja =="")
            await DisplayAlert("Dodawanie certyfikatu", "WprowadŸ dane.", "Ok");

        string answer = await _dataService.Add(ModelC);

        Dispatcher.Dispatch(() =>
        {
            if (answer == "Ok")
                OnRightSwiped();
        });

    }
    #endregion
}