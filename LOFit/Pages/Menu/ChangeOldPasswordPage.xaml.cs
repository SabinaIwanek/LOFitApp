using LOFit.DataServices.Coach;
using LOFit.DataServices.Login;
using LOFit.DataServices.User;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;
using LOFit.Tools;

namespace LOFit.Pages.Menu;

public partial class ChangeOldPasswordPage : ContentPage
{
    private readonly ILoginRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;

    #region Binding prop

    private string _oldPassword;
    public string OldPass
    {
        get { return _oldPassword; }
        set
        {
            _oldPassword = value;
            OnPropertyChanged();
        }
    }

    private string _newPassword1;
    public string NewPassword1
    {
        get { return _newPassword1; }
        set
        {
            _newPassword1 = value;
            OnPropertyChanged();
        }
    }

    private string _newPassword2;
    public string NewPassword2
    {
        get { return _newPassword2; }
        set
        {
            _newPassword2 = value;
            OnPropertyChanged();
        }
    }
    #endregion
    public ChangeOldPasswordPage(ILoginRestService dataService, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
	{
		InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
        BindingContext = this;

        OldPass = string.Empty; 
        NewPassword1 = string.Empty;
        NewPassword2 = string.Empty;
    }

    #region Menu buttons
    void OnBackClicked(object sender, EventArgs e)
    {
        OnProfileClicked(sender, e);
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

    #region Bottom buttons
    async void OnChangeButtonClicked(object sender, EventArgs e)
    {
        if (NewPassword1 == string.Empty || NewPassword2 == string.Empty || OldPass == string.Empty)
        {
            await DisplayAlert("Zmiana has³a", $"Uzupe³nij wszystkie pola.", "OK");
            return;
        }


        if (!NewPassword1.Any(char.IsDigit))
        {
            await DisplayAlert("Zmiana has³a", "Nowe has³o musi zawieraæ przynajmniej jedn¹ cyfrê.", "Ok");
            return;
        }

        if (!NewPassword1.Any(char.IsUpper))
        {
            await DisplayAlert("Zmiana has³a", "Nowe has³o zawieraæ przynajmniej jedn¹ du¿¹ literê.", "Ok");
            return;
        }

        if (NewPassword1 != NewPassword2)
        {
            await DisplayAlert("Zmiana has³a", $"Nowe has³a ró¿ni¹ siê.", "OK");
            return;
        }

        if (NewPassword1 == OldPass)
        {
            await DisplayAlert("Zmiana has³a", $"Próba zmiany has³a na starsze.", "OK");
            return;
        }

        string result = await _dataService.ChangePassword(new ChangePasswordModel() { OldPassword = OldPass, NewPassword = NewPassword1});

        if (result == "Ok")
        {
            await DisplayAlert("Zmiana has³a", $"Zmieniono has³o.", "OK");

            OnProfileClicked(sender, e);
        }
        else
        {
            await DisplayAlert("Zmiana has³a", $"Stare has³o jest niepoprawne.", "OK");
        }
    }
    #endregion
}