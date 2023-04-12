using LOFit.DataServices.Login;
using LOFit.Models;

namespace LOFit.Pages.Login;

[QueryProperty(nameof(LoginM), "LoginModel")]
public partial class ChangePasswordPage : ContentPage
{
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

    private readonly ILoginRestService _dataService;

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

    public ChangePasswordPage(ILoginRestService dataService)
    {
        InitializeComponent();

        _dataService = dataService;
        BindingContext = this;
        Password2 = string.Empty;
        LoginM = new LoginModel(true);
    }

    async void OnSendCodeButtonClicked(object sender, EventArgs e)
    {
        if (LoginM.Email == string.Empty)
        {
            Information.Text = "Wpisz e-mail.";
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

            Information.Text = $"Wys³ano kod na email {LoginM.Email}.";
        }
        else
        {
            Information.Text = result;
        }
    }
    async void OnChangeButtonClicked(object sender, EventArgs e)
    {
        Information.Text = string.Empty;

        if (LoginM.Password == null || Password2 == null || LoginM.Password == string.Empty || Password2 == string.Empty)
        {
            Information.Text = "Wpisz oba has³a.";
            return;
        }

        if (LoginM.Password != Password2)
        {
            Information.Text = "Has³a ró¿ni¹ siê.";
            return;
        }

        if (LoginM.Code > 999999 || LoginM.Code < 100000)
        {
            Information.Text = "Kod posiada 6 cyfr. Nieprawid³owa iloœæ cyfr.";
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
            Information.Text = result;
        }
    }


}