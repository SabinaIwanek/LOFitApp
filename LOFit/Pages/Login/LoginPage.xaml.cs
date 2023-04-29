using LOFit.DataServices.Coach;
using LOFit.DataServices.Login;
using LOFit.DataServices.Measurement;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Admin;
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

        BindingContext = this;
        LoginM = new LoginModel(false);
    }

    async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        Information.Text = string.Empty;

        if (_loginModel.Email == string.Empty || _loginModel.Password == string.Empty || _loginModel.Email == null || _loginModel.Password == null)
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
                Singleton.Instance.IdUsera = -1;

                await Shell.Current.GoToAsync(nameof(MeasurementPage));
                return;
            }

            if (Singleton.Instance.Type == TypKonta.Trener)
            {
                CoachModel model = await _dataServiceCoach.GetOne(-1);
                Singleton.Instance.IdTrenera = model.Id;

                await Shell.Current.GoToAsync(nameof(ConnectionsPage));
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

