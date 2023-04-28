using LOFit.DataServices.Coach;
using LOFit.DataServices.User;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;
using LOFit.Tools;

namespace LOFit.Pages.Menu;

[QueryProperty(nameof(CoachM), "CoachModel")]
[QueryProperty(nameof(UserM), "UserModel")]
public partial class EditProfilePage : ContentPage
{
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;

    #region Binding prop

    CoachModel _modelCoachM;
    public CoachModel CoachM
    {
        get { return _modelCoachM; }
        set
        {
            _modelCoachM = value;
            if (_modelCoachM != null)
            {
                SelectedGender = DataTools.ReturnGender(value.Plec);
                if (value.Nr_telefonu != null) Phone = value.Nr_telefonu.ToString();
                OnPropertyChanged();
            }
        }
    }

    UserModel _modelUserM;
    public UserModel UserM
    {
        get { return _modelUserM; }
        set
        {
            _modelUserM = value;
            if (_modelUserM != null)
            {
                SelectedGender = DataTools.ReturnGender(value.Plec);
                if (value.Nr_telefonu != null) Phone = value.Nr_telefonu.ToString();
                OnPropertyChanged();
            }
        }
    }

    private string _selectedGender;
    public string SelectedGender
    {
        get { return _selectedGender; }
        set
        {
            _selectedGender = value;
            if (Singleton.Instance.Type == TypKonta.Uzytkownik && UserM != null) UserM.Plec = DataTools.EntryGender(value);
            else if (Singleton.Instance.Type == TypKonta.Trener && CoachM != null) CoachM.Plec = DataTools.EntryGender(value);
            OnPropertyChanged();
        }
    }

    private string _phone;
    public string Phone
    {
        get { return _phone; }
        set
        {
            string phone = value?.Replace(" ", "");

            if (!char.IsDigit(phone.Last()))
            {
                OnPropertyChanged();
                return;
            }

            if (phone.Any(x => !Char.IsDigit(x)))
            {
                OnPropertyChanged();
                return;
            }

            int phoneNumber;
            try { phoneNumber = Int32.Parse(phone); }
            catch { OnPropertyChanged(); return; }

            if (phoneNumber > 999999999)
            {
                OnPropertyChanged();
                return;
            }

            _phone = phone;

            if (Singleton.Instance.Type == TypKonta.Uzytkownik) UserM.Nr_telefonu = phoneNumber;
            else if (Singleton.Instance.Type == TypKonta.Trener) CoachM.Nr_telefonu = phoneNumber;

            OnPropertyChanged();
        }
    }
    #endregion
    public EditProfilePage(ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
    {
        InitializeComponent();
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
        BindingContext = this;
        Switch();
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

    #region Switch
    private void Switch()
    {

    }
    #endregion

    #region Bottom button
    async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ChangeOldPasswordPage));
    }
    async void OnSendButtonClicked(object sender, EventArgs e)
    {
        string answer = string.Empty;
        if (Singleton.Instance.Type == TypKonta.Uzytkownik) answer = await _dataServiceUser.Update(UserM);
        else if (Singleton.Instance.Type == TypKonta.Trener) answer = await _dataServiceCoach.Update(CoachM);

        if (answer == "Ok")
        {
            OnProfileClicked(sender, e);
        }
        else
        {
            await DisplayAlert("Zmiana danych", $"B³¹d {answer}", "OK");
        }
    }
    #endregion

}