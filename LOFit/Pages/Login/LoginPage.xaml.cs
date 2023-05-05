using LOFit.DataServices.Coach;
using LOFit.DataServices.Login;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Admin;
using LOFit.Pages.Admin.VerifyLists;
using LOFit.Pages.Coachs;
using LOFit.Pages.Measures;
using LOFit.Tools;

namespace LOFit.Pages.Login;

[QueryProperty(nameof(LoginM), "LoginModel")]
public partial class LoginPage : ContentPage
{
    private readonly ILoginRestService _dataServiceLogin;
    private readonly ICoachRestService _dataServiceCoach;

    private LoginModel _loginModel;
    public LoginModel LoginM
    {
        get { return _loginModel; }
        set
        {
            _loginModel = value;
            OnPropertyChanged();
        }
    }

    public LoginPage(ILoginRestService dataServiceLogin, ICoachRestService dataServiceCoach)
    {
        InitializeComponent();

        _dataServiceLogin = dataServiceLogin;
        _dataServiceCoach = dataServiceCoach;

        Singleton.Instance.IdTrenera = 0;
        Singleton.Instance.IdUsera = 0;
        Singleton.Instance.Type = null;
        Singleton.Instance.PlanDay = 0;
        Singleton.Instance.Token = "";
        Singleton.Instance.DateToShow = DateTime.Now;

        BindingContext = this;
        LoginM = new LoginModel(false);
    }

    async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        if (_loginModel.Email == string.Empty || _loginModel.Password == string.Empty || _loginModel.Email == null || _loginModel.Password == null)
        {
            await DisplayAlert("Logowanie", "Wprowadź dane do obu pól.", "Ok");

            return;
        }

        string result = await _dataServiceLogin.Login(LoginM);

        if (result == "Ok")
        {
            if (Singleton.Instance.Type == TypKonta.Administrator)
            {
                await Shell.Current.GoToAsync(nameof(VerifyAppUsersPage));
                return;
            }

            if (Singleton.Instance.Type == TypKonta.Uzytkownik)
            {
                Singleton.Instance.IdUsera = -1;

                await Shell.Current.GoToAsync(nameof(MeasurementPage));
                return;
            }

            if (Singleton.Instance.Type == TypKonta.Trener)
            {
                CoachModel model = await _dataServiceCoach.GetOne(-1);
                Singleton.Instance.IdTrenera = model.Id;
                Singleton.Instance.CoachMinutes = model.Czas_uslugi_min;

                await Shell.Current.GoToAsync(nameof(ConnectionsPage));
                return;
            }
        }
        else
        {
            await DisplayAlert("Logowanie", result, "Ok");
        }
    }

    async void OnRegistrationButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegistrationPage));
    }

    async void OnSendCodeButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ChangePasswordPage));
    }
}

