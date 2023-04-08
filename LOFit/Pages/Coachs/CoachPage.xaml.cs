using LOFit.DataServices.Coach;
using LOFit.Models;
using LOFit.Pages.Menu;
using LOFit.Tools;
using Microsoft.Maui.Controls;

namespace LOFit.Pages.Coachs;

[QueryProperty(nameof(CoachM), "CoachModel")]
public partial class CoachPage : ContentPage
{
    private readonly ICoachRestService _dataService;

    #region Binding prop

    CoachModel _model;
    public CoachModel CoachM
    {
        get { return _model; }
        set
        {
            _model = value;
            if (_model != null)
            {
                Wizytowka = _model.Wizytowka();
                Ocena = _model.Ocena();
                TypTrenera = _model.TypTrenera();
                CenaUslugi = _model.CenaUslugi();
            }
            OnPropertyChanged();
        }
    }

    string _wizytowka;
    public string Wizytowka
    {
        get { return _wizytowka; }
        set { _wizytowka = value; OnPropertyChanged(); }
    }

    string _ocena;
    public string Ocena
    {
        get { return _ocena; }
        set { _ocena = value; OnPropertyChanged(); }
    }

    string _typTrenera;
    public string TypTrenera
    {
        get { return _typTrenera; }
        set { _typTrenera = value; OnPropertyChanged(); }
    }

    string _cenaUslugi;
    public string CenaUslugi
    {
        get { return _cenaUslugi; }
        set { _cenaUslugi = value; OnPropertyChanged(); }
    }
    #endregion

    public CoachPage(ICoachRestService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        BindingContext = this;

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) =>
        {
            Shell.Current.GoToAsync(nameof(CoachsPage));
        };

        Content.GestureRecognizers.Add(swipeGestureRight);
        #endregion
    }

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

    #region Lists
    async void OnLoadListClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var property = button.CommandParameter.ToString();
        if (property == "Certyf") ;
        if (property == "Opinie") ;
        if (property == "Kalend") ;
    }
    async void ListLoadCertyf()
    {
        collectionView.ItemsSource = await _dataService.();
    }
    #endregion
}