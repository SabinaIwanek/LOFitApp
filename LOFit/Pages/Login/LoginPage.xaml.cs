using LOFit.DataServices.Login;
using LOFit.DataServices.Measurement;
using LOFit.Enums;
using LOFit.Models;
using LOFit.Pages.Admin;
using LOFit.Pages.Central;
using LOFit.Pages.Measures;
using LOFit.Tools;

namespace LOFit.Pages.Login;

[QueryProperty(nameof(LoginM), "LoginModel")]
public partial class LoginPage : ContentPage
{
    private readonly ILoginRestService _dataServiceLogin;

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

    public LoginPage(ILoginRestService dataServiceLogin)
    {
        InitializeComponent();

        _dataServiceLogin = dataServiceLogin;
        BindingContext = this;
        LoginM = new LoginModel(false);
    }

    async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        if (_loginModel.Email == string.Empty || _loginModel.Password == string.Empty)
        {
            Information.Text = "Wprowadź dane do obu pól.";
            return;
        }

        string result = await _dataServiceLogin.Login(LoginM);

        if (result == "Ok")
        {
            if (Singleton.Instance.Type == TypKonta.Administrator)
            {
                await Shell.Current.GoToAsync(nameof(CentralAdminPage));
                return;
            }

            if (Singleton.Instance.Type == TypKonta.Uzytkownik)
            {
                await Shell.Current.GoToAsync(nameof(MeasurementPage));
                return;
            }

            if (Singleton.Instance.Type == TypKonta.Trener)
            {
                await Shell.Current.GoToAsync(nameof(CentralCoachPage));
                return;
            }
        }
        else
        {
            Information.Text = result;
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

