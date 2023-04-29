using LOFit.DataServices.Coach;
using LOFit.Tools;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;
using LOFit.Pages.Measures;
using LOFit.DataServices.User;
using LOFit.Resources.Styles;

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

        EditImageButton.IsVisible = Singleton.Instance.Type == TypKonta.Uzytkownik;

        BindingContext = this;
    }

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
}