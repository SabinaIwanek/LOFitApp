using LOFit.DataServices.Login;
using LOFit.DataServices.Registration;
using LOFit.Models.Accounts;
using LOFit.Resources.Styles;
using LOFit.Tools;

namespace LOFit.Pages.Login;

[QueryProperty(nameof(UserM), "RegistrationModel")]
public partial class RegistrationPage : ContentPage
{
    #region Binding prop
    private string _selectedGender;
    public string SelectedGender
    {
        get { return _selectedGender; }
        set
        {
            _selectedGender = value;
             UserM.Plec = DataTools.EntryGender(value);
            OnPropertyChanged();
        }
    }

    private string _selectedCoach;
    public string SelectedCoach
    {
        get { return _selectedCoach; }
        set
        {
            _selectedCoach = value;
             UserM.Typ_trenera = DataTools.EntryCoach(value);
            OnPropertyChanged();
        }
    }

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
            UserM.Nr_telefonu = phoneNumber;

            OnPropertyChanged();
        }
    }

    private bool IsUser = true;

    private readonly IRegistrationRestService _dataService;

    private RegistrationModel _registrationModel;
    public RegistrationModel UserM
    {
        get { return _registrationModel; }
        set
        {
            _registrationModel = value;
            OnPropertyChanged();
        }
    }
    #endregion

    public RegistrationPage(IRegistrationRestService dataService)
    {
        InitializeComponent();

        _dataService = dataService;
        BindingContext = this;
        UserM = new RegistrationModel();
    }

    #region Choice Buttons
    async void OnUserButtonClicked(object sender, EventArgs e)
    {
        MiejscowoscLabel.IsVisible = false;
        MiejscowoscEntry.IsVisible = false;
        Typ_treneraLabel.IsVisible = false;
        CoachPicker.IsVisible = false;
        IsUser = true;

        //wizualne
        UserButton.TextColor = MyColors.Primary;
        BottomUserButton.BackgroundColor = MyColors.Primary;

        CoachButton.TextColor = MyColors.MyText;
        BottomCoachButton.BackgroundColor = Colors.Transparent;
    }
    async void OnCoachButtonClicked(object sender, EventArgs e)
    {
        MiejscowoscLabel.IsVisible = true;
        MiejscowoscEntry.IsVisible = true;
        Typ_treneraLabel.IsVisible = true;
        CoachPicker.IsVisible = true;
        IsUser = false;

        //wizualne
        CoachButton.TextColor = MyColors.Primary;
        BottomCoachButton.BackgroundColor = MyColors.Primary;

        UserButton.TextColor = MyColors.MyText;
        BottomUserButton.BackgroundColor = Colors.Transparent;
    }
    #endregion

    #region Bottom button
    async void OnSendButtonClicked(object sender, EventArgs e)
    {
        Information.Text = string.Empty;

        if (!DataTools.CheckEmail(UserM.Email))
        {
            Information.Text = "B³êdny adres email";
            return;
        }

        if (Password2 == string.Empty || UserM.Haslo == string.Empty || Password2 == null || UserM.Haslo == null)
        {
            Information.Text = "Wpisz has³o do dwóch pól.";
            return;
        }

        if (Password2 != UserM.Haslo)
        {
            Information.Text = "Has³a siê ró¿ni¹.";
            return;
        }

        if (UserM.Imie == string.Empty || UserM.Imie == null)
        {
            Information.Text = "Wpisz imiê.";
            return;
        }

        string answer = string.Empty;

        if (IsUser) answer = await _dataService.CreateUser(UserM);
        else answer = await _dataService.CreateCoach(UserM);

        if (answer == "Ok")
        {
            var navigationParameter = new Dictionary<string, object>
             {
                {nameof(LoginModel), new LoginModel(false){Email = UserM.Email } }
             };

            await Shell.Current.GoToAsync("Login", navigationParameter);
        }
        else
        {
            Information.Text = answer;
        }
    }
    #endregion
}