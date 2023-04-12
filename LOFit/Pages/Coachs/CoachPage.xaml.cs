using LOFit.DataServices.Certificate;
using LOFit.DataServices.Coach;
using LOFit.DataServices.Connection;
using LOFit.Enums;
using LOFit.Models;
using LOFit.Pages.Menu;
using LOFit.Tools;

namespace LOFit.Pages.Coachs;

[QueryProperty(nameof(CoachM), "CoachModel")]
public partial class CoachPage : ContentPage
{
    private readonly ICoachRestService _dataService;
    private readonly ICertificateRestService _dataServiceCert;
    private readonly IOpinionRestService _dataServiceOpinion;
    private readonly IConnectionRestService _dataServiceConn;
    private List<Button> _buttons;

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
                Wizytowka = _model.Wizytowka();
                Ocena = _model.Ocena();
                TypTrenera = _model.TypTrenera();
                CenaUslugi = _model.CenaUslugi();
                ListLoadCertyf();
                if (Singleton.Instance.Type == TypKonta.Uzytkownik) LoadMyOpinion();
            }
            OnPropertyChanged();
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

    public CoachPage(ICoachRestService dataService, ICertificateRestService dataServiceCert, IOpinionRestService dataServiceOpinion, IConnectionRestService dataServiceConn)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCert = dataServiceCert;
        _dataServiceOpinion = dataServiceOpinion;
        _dataServiceConn = dataServiceConn;

        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3 };

        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            ButtonSelect.IsVisible= false;
        }

        #region Swipe right
            SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) =>
        {
            Shell.Current.GoToAsync(nameof(CoachsPage));
        };

        Content.GestureRecognizers.Add(swipeGestureRight);
        _dataServiceOpinion = dataServiceOpinion;
        #endregion
    }

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

    #region Lists
    async void OnLoadListClicked(object sender, EventArgs e)
    {
        DataTools.ButtonNotClicked(_buttons);

        var button = (Button)sender;
        var property = button.CommandParameter.ToString();

        if (property == "Certyf")
        {
            collectionViewOpinie.IsVisible = false;
            collectionViewCertyf.IsVisible = true;
            GridDodajOpinie.IsVisible = false;
            DataTools.ButtonClicked(_buttons[0]);

            ListLoadCertyf();
        }
        if (property == "Opinie")
        {
            collectionViewOpinie.IsVisible = true;
            collectionViewCertyf.IsVisible = false;
            if (Singleton.Instance.Type == TypKonta.Uzytkownik) GridDodajOpinie.IsVisible = true;
            
            DataTools.ButtonClicked(_buttons[1]);

            ListLoadOpinie();
        }
        if (property == "Kalend")
        {
            collectionViewOpinie.IsVisible = false;
            collectionViewCertyf.IsVisible = false;
            DataTools.ButtonClicked(_buttons[2]);
        }
    }

    #region Certyfikaty
    async void ListLoadCertyf()
    {
        collectionViewCertyf.ItemsSource = await _dataServiceCert.GetCoachList(CoachM.Id);
    }

    // Trenerzy
    async void OnCertClicked(object sender, SelectionChangedEventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Trener)
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
    #endregion

    #region Opinie
    async void ListLoadOpinie()
    {
        collectionViewOpinie.ItemsSource = await _dataServiceOpinion.GetCoachList(CoachM.Id);
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
        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            bool result = await DisplayAlert("Zg³oœ opiniê", "Czy chcesz zg³osiæ opiniê?", "Tak", "Nie");

            if (result)
            {
                OpinionModel model = e.CurrentSelection.FirstOrDefault() as OpinionModel;
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
        bool result = await DisplayAlert("Wybierz trenera", "Czy chcesz, aby wybrany trener mia³ podgl¹d do Twoich danych?", "Tak", "Nie");

        if(result)
        {
            ConnectionModel model = new ConnectionModel();
            model.Id_trenera = CoachM.Id;
            model.Czas_od = DateTime.Now;
            model.Zatwierdzone = 0;

            await _dataServiceConn.Add(model);
        }
    }
    #endregion
}