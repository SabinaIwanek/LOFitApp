using LOFit.DataServices.Certificate;
using LOFit.DataServices.Coach;
using LOFit.DataServices.Connection;
using LOFit.DataServices.User;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.ProfileMenu;
using LOFit.Pages.Menu;
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
    private int _type;
    private bool _isUserProfile;

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
                Ocena = _model.Ocena();
                TypTrenera = _model.TypTrenera();
                CenaUslugi = _model.CenaUslugi();
                ListLoadCertyf();

                if (Singleton.Instance.Type == TypKonta.Uzytkownik)
                {
                    LoadMyOpinion();
                    LoadOnSelectButtonClickedText();
                    GridDodajOpinie.IsVisible = false;
                    ButtonAddCert.IsVisible = false;
                    ButtonSelect.IsVisible = true;
                    ButtonOpisEdit.IsVisible = false;
                }
                else if (Singleton.Instance.Type == TypKonta.Trener && _isUserProfile)
                {
                    GridDodajOpinie.IsVisible = false;
                    ButtonAddCert.IsVisible = true;
                    ButtonSelect.IsVisible = false;
                    ButtonOpisEdit.IsVisible = true;
                }
                else if (Singleton.Instance.Type == TypKonta.Administrator)
                {
                    GridDodajOpinie.IsVisible = false;
                    ButtonAddCert.IsVisible = false;
                    ButtonSelect.IsVisible = false;
                    ButtonOpisEdit.IsVisible = false;
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

    string _ocena;
    public string Ocena
    {
        get { return _ocena; }
        set { _ocena = value; OnPropertyChanged(); }
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
    }
    async void OnLeftSwiped()
    {
        if (Singleton.Instance.Type == TypKonta.Trener)
            await Shell.Current.GoToAsync(nameof(ConnectionsPage));
    }
    #endregion

    #region Menu buttons
    async void OnCoachsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CoachsPage));
    }
    async void OnProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }
    async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsUserPage));
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

    async void OnEditCicked(object sender, EventArgs e)
    {
        EntryOpisEdit.IsReadOnly = !EntryOpisEdit.IsReadOnly;

        if (EntryOpisEdit.IsReadOnly)
        {
            EntryOpisEdit.BackgroundColor = Colors.Transparent;

            string answer = await _dataService.Update(CoachM);

            if (answer == "Ok") ButtonOpisEdit.Text = "Zmieñ opis";
        }
        else
        {
            EntryOpisEdit.BackgroundColor = MyColors.MyEntryBg;
            ButtonOpisEdit.Text = "Zapisz";
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
        /*if (property == "Kalend")
        {
            collectionViewOpinie.IsVisible = false;
            collectionViewCertyf.IsVisible = false;
            GridDodajOpinie.IsVisible = false;
            ButtonAddCert.IsVisible = false;

            DataTools.ButtonClicked(_buttons[3], _grids[3]);
        }*/
    }

    #region Certyfikaty
    async void ListLoadCertyf()
    {
        collectionViewCertyf.ItemsSource = await _dataServiceCert.GetCoachList(CoachM.Id);
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
        collectionViewOpinie.ItemsSource = await ListModelTools.ReturnOpinionList( await _dataServiceOpinion.GetCoachList(CoachM.Id), _dataServiceUser);
    }
    async void LoadMyOpinion()
    {
        Opinia = await _dataServiceOpinion.GetMyOpinion(CoachM.Id);
    }

    // Users
    async void OnSendOpinionClicked(object sender, EventArgs e)
    {
        Opinia.Id_trenera = CoachM.Id;
        if (Opinia.Opis_zgloszenia == null) Opinia.Opis_zgloszenia = "";

        if (Opinia.Id == 0) await _dataServiceOpinion.Add(Opinia);
        else await _dataServiceOpinion.Update(Opinia);

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
}