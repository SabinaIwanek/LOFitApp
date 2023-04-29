using LOFit.DataServices.Certificate;
using LOFit.DataServices.Coach;
using LOFit.DataServices.Connection;
using LOFit.DataServices.User;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.ProfileMenu;
using LOFit.Pages.Meals;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Pages.Workouts;
using LOFit.Resources.Styles;
using LOFit.Tools;

namespace LOFit.Pages.Coachs;

[QueryProperty(nameof(CoachM), "CoachModel")]
public partial class CoachPage : ContentPage
{
    private readonly ICoachRestService _dataService;
    private readonly ICertificateRestService _dataServiceCert;
    private readonly IOpinionRestService _dataServiceOpinion;
    private readonly IConnectionRestService _dataServiceConn;
    private readonly IUserRestService _dataServiceUser;
    private List<Button> _buttons;
    private List<Grid> _grids;
    private List<Image> _stars;
    private List<ImageButton> _starsButton;
    private int _type;
    private bool _isUserProfile;

    #region Binding function pom
    async void ReturnOpinion()
    {
        (double, string) ocena = await _model.Ocena(_dataServiceOpinion);

        Ocena = ocena.Item1;
        OcenaString = ocena.Item2;

        DataTools.StarsImage(_stars, Ocena);
    }
    #endregion

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
                _isUserProfile = _model.Id == Singleton.Instance.IdTrenera;

                Wizytowka = _model.Wizytowka();
                ReturnOpinion();
                TypTrenera = _model.TypTrenera();
                CenaUslugi = _model.CenaUslugi();
                DataUr = _model.DataUr();
                TelefonString = _model.TelefonString();
                ListLoadCertyf();

                if (Singleton.Instance.Type == TypKonta.Uzytkownik)
                {
                    LoadMyOpinion();
                    LoadOnSelectButtonClickedText();
                    GridDodajOpinie.IsVisible = false;
                    ButtonAddCert.IsVisible = false;
                    ButtonSelect.IsVisible = true;
                    ButtonEdit.IsVisible = false;
                    CoachBottomMenu.IsVisible = false;
                }
                else if (Singleton.Instance.Type == TypKonta.Trener && _isUserProfile)
                {
                    GridDodajOpinie.IsVisible = false;
                    ButtonSelect.IsVisible = false;
                    ButtonAddCert.IsVisible = true;
                    ButtonEdit.IsVisible = true;
                    CoachBottomMenu.IsVisible = true;
                    ImageButtonProfile.BackgroundColor = MyColors.Primary;
                }
                else if (Singleton.Instance.Type == TypKonta.Trener)
                {
                    GridDodajOpinie.IsVisible = false;
                    ButtonSelect.IsVisible = false;
                    ButtonAddCert.IsVisible = false;
                    ButtonEdit.IsVisible = false;
                    CoachBottomMenu.IsVisible = true;

                }
                else if (Singleton.Instance.Type == TypKonta.Administrator)
                {
                    GridDodajOpinie.IsVisible = false;
                    ButtonAddCert.IsVisible = false;
                    ButtonSelect.IsVisible = false;
                    ButtonEdit.IsVisible = false;
                    CoachBottomMenu.IsVisible = false;
                }

                OnPropertyChanged();
            }
        }
    }

    OpinionModel _opinia;
    public OpinionModel Opinia
    {
        get { return _opinia; }
        set
        {
            _opinia = value;
            if (_opinia == null || _opinia.Id == 0)
            {
                ButtonSaveOpinion.Text = "Dodaj";
            }
            else
            {
                ButtonSaveOpinion.Text = "Zmieñ";
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

    double _ocena;
    public double Ocena
    {
        get { return _ocena; }
        set { _ocena = value; OnPropertyChanged(); }
    }

    string _ocenaString;
    public string OcenaString
    {
        get { return _ocenaString; }
        set { _ocenaString = value; OnPropertyChanged(); }
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
    string _dataUr;
    public string DataUr
    {
        get { return _dataUr; }
        set { _dataUr = value; OnPropertyChanged(); }
    }
    string _telefonString;
    public string TelefonString
    {
        get { return _telefonString; }
        set { _telefonString = value; OnPropertyChanged(); }
    }
    #endregion

    public CoachPage(ICoachRestService dataService, ICertificateRestService dataServiceCert, IOpinionRestService dataServiceOpinion, IConnectionRestService dataServiceConn, IUserRestService dataServiceUser)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCert = dataServiceCert;
        _dataServiceOpinion = dataServiceOpinion;
        _dataServiceConn = dataServiceConn;
        _dataServiceUser = dataServiceUser;

        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2 };
        _grids = new List<Grid>() { BottomButton1, BottomButton2 };
        _stars = new List<Image>() { Star1, Star2, Star3, Star4, Star5 };
        _starsButton = new List<ImageButton>() { StarIB1, StarIB2, StarIB3, StarIB4, StarIB5 };

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) => OnRightSwiped();

        Content.GestureRecognizers.Add(swipeGestureRight);
        #endregion

        #region Swipe left
        SwipeGestureRecognizer swipeGestureLeft = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Left
        };

        swipeGestureLeft.Swiped += (s, e) => OnLeftSwiped();

        Content.GestureRecognizers.Add(swipeGestureLeft);
        #endregion
    }
    #region Swipe
    async void OnRightSwiped()
    {
        if (!_isUserProfile) await Shell.Current.GoToAsync(nameof(CoachsPage));
        else
        {
            if (Singleton.Instance.Type == TypKonta.Trener)
                await Shell.Current.GoToAsync(nameof(ConnectionsPage));
        }
    }
    async void OnLeftSwiped()
    {
        if (Singleton.Instance.Type == TypKonta.Trener)
            await Shell.Current.GoToAsync(nameof(CoachsPage));
    }
    #endregion

    #region Menu buttons
    async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CoachsPage));
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
            CoachModel model = await _dataService.GetOne(-1);
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

    #region Coach Info
    async void OnEditButtonClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Trener && _isUserProfile)
        {
            var navigationParameter = new Dictionary<string, object>
                {
                    { nameof(CoachModel), CoachM} ,
                    { nameof(UserModel), new UserModel()}
                };

            await Shell.Current.GoToAsync(nameof(EditProfilePage), navigationParameter);
        }
    }
    #endregion

    #region Lists
    void OnLoadListClicked(object sender, EventArgs e)
    {
        DataTools.ButtonNotClicked(_buttons, _grids);

        var button = (Button)sender;
        var property = button.CommandParameter.ToString();

        if (property == "Certyf")
        {
            collectionViewOpinie.IsVisible = false;
            collectionViewCertyf.IsVisible = true;
            GridDodajOpinie.IsVisible = false;
            if (Singleton.Instance.Type == TypKonta.Trener && _isUserProfile) ButtonAddCert.IsVisible = true;

            DataTools.ButtonClicked(_buttons[0], _grids[0]);

            ListLoadCertyf();
        }
        if (property == "Opinie")
        {
            collectionViewOpinie.IsVisible = true;
            collectionViewCertyf.IsVisible = false;
            if (Singleton.Instance.Type == TypKonta.Uzytkownik) GridDodajOpinie.IsVisible = true;
            ButtonAddCert.IsVisible = false;

            DataTools.ButtonClicked(_buttons[1], _grids[1]);

            ListLoadOpinie();
        }
    }

    #region Certyfikaty
    async void ListLoadCertyf()
    {
        collectionViewCertyf.ItemsSource = ListModelTools.ReturnCertificateList(await _dataServiceCert.GetCoachList(CoachM.Id));
    }

    // Trenerzy
    async void OnCertClicked(object sender, SelectionChangedEventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Trener && _isUserProfile)
        {
            bool result = await DisplayAlert("Usuñ certyfikat", "Czy chcesz usun¹æ certyfikat?", "Tak", "Nie");

            if (result)
            {
                CertificateModel model = e.CurrentSelection.FirstOrDefault() as CertificateModel;
                await _dataServiceCert.Delete(model.Id);
            }
            ListLoadCertyf();
        }
    }
    async void OnAddCertClcked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Trener && _isUserProfile)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                    { nameof(CertificateModel), new CertificateModel() }
            };

            await Shell.Current.GoToAsync(nameof(CertificatePage), navigationParameter);
        }
    }
    #endregion

    #region Opinie
    async void ListLoadOpinie()
    {
        collectionViewOpinie.ItemsSource = await ListModelTools.ReturnOpinionList(await _dataServiceOpinion.GetCoachList(CoachM.Id), _dataServiceUser);
    }
    async void LoadMyOpinion()
    {
        Opinia = await _dataServiceOpinion.GetMyOpinion(CoachM.Id);

        DataTools.StarsImageButton(_starsButton, Opinia.Ocena);
    }

    // Users
    void OnStarClicked(object sender, EventArgs e)
    {
        ImageButton button = (ImageButton)sender;
        int parametr = Int32.Parse((string)button.CommandParameter);

        Opinia.Ocena = parametr;
        DataTools.StarsImageButton(_starsButton, parametr);
    }
    async void OnSendOpinionClicked(object sender, EventArgs e)
    {
        Opinia.Id_trenera = CoachM.Id;
        if (Opinia.Opis_zgloszenia == null) Opinia.Opis_zgloszenia = "";

        if (Opinia.Ocena == 0) Opinia.Ocena = 1;

        if (Opinia.Id == 0) await _dataServiceOpinion.Add(Opinia);
        else await _dataServiceOpinion.Update(Opinia);

        ReturnOpinion();
        LoadMyOpinion();
        ListLoadOpinie();
    }
    // Trenerzy
    async void OnOpinionClicked(object sender, SelectionChangedEventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Trener && _isUserProfile)
        {
            bool result = await DisplayAlert("Zg³oœ opiniê", "Czy chcesz zg³osiæ opiniê?", "Tak", "Nie");

            if (result)
            {
                OpinionListModel listModel = e.CurrentSelection.FirstOrDefault() as OpinionListModel;
                OpinionModel model = listModel.Opinion;
                model.Zgloszona = 1;
                await _dataServiceOpinion.Update(model);
            }
            ListLoadOpinie();
        }
    }
    #endregion

    #endregion

    #region Bottom button

    #region Trener
    async void OnBottomMenuClicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        var parameter = (string)button.CommandParameter;

        if (parameter == "meals")
        {
            await Shell.Current.GoToAsync(nameof(MealsPage));
        }
        else if (parameter == "measure")
        {
            await Shell.Current.GoToAsync(nameof(MeasurementPage));
        }
        else if (parameter == "workout")
        {
            await Shell.Current.GoToAsync(nameof(WorkoutsPage));
        }
        else if (parameter == "coachs")
        {
            await Shell.Current.GoToAsync(nameof(CoachsPage));
        }
        else if (parameter == "coachsProfile")
        {
            OnProfileClicked(sender, e);
        }
        else if (parameter == "connections")
        {
            if (Singleton.Instance.Type == TypKonta.Trener)
                await Shell.Current.GoToAsync(nameof(ConnectionsPage));
        }
        else if (parameter == "plans")
        {
        }
        else if (parameter == "calendar")
        {
        }
    }
    #endregion

    #region User
    async void OnSelectButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert($"{ButtonSelect.Text}", "Czy aby na pewno?", "Tak", "Nie");

        if (result)
        {
            if (_type == 0)
            {
                List<ConnectionModel> connections = await _dataServiceConn.GetUserList(-1);
                Dispatcher.Dispatch(() =>
                {
                    if (!connections.Any()) return;
                    int? id = connections.Where(x => x.Id_trenera == CoachM.Id && x.Zatwierdzone == 0).Select(x => x.Id).FirstOrDefault();

                    if (id == null) return;
                    _dataServiceConn.Delete((int)id);
                });
            }
            if (_type == 1)
            {
                List<ConnectionModel> connections = await _dataServiceConn.GetUserList(-1);
                Dispatcher.Dispatch(() =>
                {
                    if (!connections.Any()) return;

                    ConnectionModel model = connections.Where(x => x.Id_trenera == CoachM.Id && x.Zatwierdzone == 1 && (x.Czas_do == null || x.Czas_do >= DateTime.Now)).FirstOrDefault();
                    if (model == null) return;

                    model.Czas_do = DateTime.Now;

                    _dataServiceConn.Update(model);
                });
            }
            if (_type == 2 || _type == 3)
            {
                ConnectionModel model = new ConnectionModel();
                model.Id_trenera = CoachM.Id;
                model.Czas_od = DateTime.Now;
                model.Zatwierdzone = 0;

                await _dataServiceConn.Add(model);
            }
        }
        Dispatcher.Dispatch(() =>
        {
            LoadOnSelectButtonClickedText();
        });
    }
    async void LoadOnSelectButtonClickedText()
    {
        _type = await _dataServiceConn.GetCoachState(CoachM.Id);

        Dispatcher.Dispatch(() =>
        {
            if (_type == 4) return;
            if (_type == 0) ButtonSelect.Text = "Cofnij proœbê";
            if (_type == 1) ButtonSelect.Text = "Zakoñcz powi¹zanie";
            if (_type == 2) ButtonSelect.Text = "Ponów proœbê (ostatnia odrzucona)";
            if (_type == 3) ButtonSelect.Text = "Wybierz trenera";
        });
    }
    #endregion

    #endregion
}