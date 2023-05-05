using LOFit.DataServices.Login;
using LOFit.Models.Accounts;
using LOFit.Tools;

namespace LOFit.Pages.Login;

[QueryProperty(nameof(LoginM), "LoginModel")]
public partial class ChangePasswordPage : ContentPage
{
    private readonly ILoginRestService _dataService;

    #region Binding prop
    private string _password2;
    public string Password2
    {
        get { return _password2; }
        set
        {
            _password2 = value;
            OnPropertyChanged();
        }
    }

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
    #endregion

    public ChangePasswordPage(ILoginRestService dataService)
    {
        InitializeComponent();

        _dataService = dataService;
        BindingContext = this;
        Password2 = string.Empty;
        LoginM = new LoginModel(true);
    }

    #region Menu buttons
    async void OnBackClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            {nameof(LoginModel), new LoginModel(false)}
        };

        Singleton.Logout();
        await Shell.Current.GoToAsync("Login", navigationParameter);
    }
    #endregion

    async void OnSendCodeButtonClicked(object sender, EventArgs e)
    {
        if (LoginM.Email == string.Empty)
        {
            await DisplayAlert("Zmiana has�a", "Wpisz e-mail.", "Ok");
            return;
        }

        string result = await _dataService.SendMail(LoginM.Email);

        if (result == "Ok")
        {
            EmailEntry.IsVisible = false;
            PasswordLabel.IsVisible = true;
            PasswordEntry.IsVisible = true;
            PasswordLabel2.IsVisible = true;
            PasswordEntry2.IsVisible = true;
            CodeLabel.IsVisible = true;
            CodeEntry.IsVisible = true;
            ChangeButton.IsVisible = true;
            CodeButton.IsVisible = false;

            await DisplayAlert("Zmiana has�a", $"Wys�ano kod na email {LoginM.Email}.", "Ok");
        }
        else
        {
            await DisplayAlert("Zmiana has�a", result, "Ok");
        }
    }
    async void OnChangeButtonClicked(object sender, EventArgs e)
    {
        if (LoginM.Password == null || Password2 == null || LoginM.Password == string.Empty || Password2 == string.Empty)
        {
            await DisplayAlert("Zmiana has�a", "Wpisz oba has�a.", "Ok");
            return;
        }

        if (LoginM.Password != Password2)
        {
            await DisplayAlert("Zmiana has�a", "Has�a r�ni� si�.", "Ok");
            return;
        }

        if (LoginM.Code > 999999 || LoginM.Code < 100000)
        {
            await DisplayAlert("Zmiana has�a", "Kod posiada 6 cyfr. Nieprawid�owa ilo�� cyfr.", "Ok");
            return;
        }

        string result = await _dataService.Login(LoginM);

        if (result == "Ok")
        {
            var navigationParameter = new Dictionary<string, object>
             {
                {nameof(LoginModel), new LoginModel(false){Email = LoginM.Email } }
             };

            await Shell.Current.GoToAsync("Login", navigationParameter);
        }
        else
        {
            await DisplayAlert("Zmiana has�a", result, "Ok");
        }
    }
}