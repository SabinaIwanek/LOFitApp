using LOFit.DataServices.Admin;
using LOFit.DataServices.Certificate;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;
using LOFit.Tools;

namespace LOFit.Pages.Admin.VerifyLists;

public partial class VerifyCoachPage : ContentPage
{
    private IAdminRestService _dataService;
    private IOpinionRestService _dataServiceOpinion;
    private List<Button> _buttons;
    private List<Grid> _grids;

    #region Binding prop
    private string _adminName;
    public string AdminName
    {
        get { return _adminName; }
        set
        {
            _adminName = value;
            OnPropertyChanged();
        }
    }

    private int _infoCoachs;
    public int InfoCoachs
    {
        get { return _infoCoachs; }
        set
        {
            _infoCoachs = value;
            OnPropertyChanged();
        }
    }

    private int _infoCertificate;
    public int InfoCertificate
    {
        get { return _infoCertificate; }
        set
        {
            _infoCertificate = value;
            OnPropertyChanged();
        }
    }

    private int _infoVerifyOpinion;
    public int InfoVerifyOpinion
    {
        get { return _infoVerifyOpinion; }
        set
        {
            _infoVerifyOpinion = value;
            OnPropertyChanged();
        }
    }

    private int _infoProducts;
    public int InfoProducts
    {
        get { return _infoProducts; }
        set
        {
            _infoProducts = value;
            OnPropertyChanged();
        }
    }
    #endregion
    public VerifyCoachPage(IAdminRestService dataService, IOpinionRestService dataServiceOpinion)
	{
		InitializeComponent();
        _dataService = dataService;
        _dataServiceOpinion = dataServiceOpinion;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3 };
        _grids = new List<Grid>() { Bottom1, Bottom2, Bottom3 };

        ListLoad(0); 
        OnLoadData();
    }

    #region Load data
    async void OnLoadData()
    {
        AdminModel admin = await _dataService.GetOne(-1);
        AdminName = $"{admin.Imie} {admin.Nazwisko}";

        InfoCoachs = (await _dataService.GetWgTypeCoach(0)).Count;
        InfoCertificate = (await _dataService.GetWgTypeCert(0)).Count;
        InfoVerifyOpinion = (await _dataService.GetWgTypeOpinion(1)).Count;
        InfoProducts = (await _dataService.GetWgTypeProducts(0)).Count;

        InfoCoach.IsVisible = InfoCoachs != 0;
        InfoCert.IsVisible = InfoCertificate != 0;
        InfoOpinion.IsVisible = InfoVerifyOpinion != 0;
        InfoProd.IsVisible = InfoProducts != 0;
    }
    #endregion

    #region Menu buttons
    async void OnButtonMenuClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var parameter = (string)button.CommandParameter;

        if (parameter == "users")
        {
            await Shell.Current.GoToAsync(nameof(VerifyAppUsersPage));
        }
        else if (parameter == "coachs")
        {
            await Shell.Current.GoToAsync(nameof(VerifyCoachPage));
        }
        else if (parameter == "ceftificate")
        {
            await Shell.Current.GoToAsync(nameof(VerifyCertificatePage));
        }
        else if (parameter == "opinion")
        {
            await Shell.Current.GoToAsync(nameof(VerifyOpinionPage));
        }
        else if (parameter == "products")
        {
            await Shell.Current.GoToAsync(nameof(VerifyProductsPage));
        }
        else if (parameter == "wyloguj")
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
    }
    #endregion

    #region Type buttons
    async void OnTypeClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var property = Int32.Parse((string)button.CommandParameter);

        ListLoad(property);
    }
    #endregion

    #region Action buttons
    async void OnOkButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var property = (int)button.CommandParameter;

        string wynik = await _dataService.SetCoach(property, 1);
    }
    async void OnNoButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var property = (int)button.CommandParameter;

        string wynik = await _dataService.SetCoach(property, 2);
    }
    #endregion

    #region List
    async void ListLoad(int type)
    {
        collectionView.ItemsSource = await ListModelTools.ReturnCoachList( await _dataService.GetWgTypeCoach(type), _dataServiceOpinion);

        DataTools.ButtonNotClicked(_buttons, _grids);
        DataTools.ButtonClicked(_buttons[type], _grids[type]);
    }
    async void OnCoachClicked(object sender, SelectionChangedEventArgs e)
    {
        CoachModel coach = e.CurrentSelection.FirstOrDefault() as CoachModel;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CoachModel), coach }
        };

        await Shell.Current.GoToAsync(nameof(CoachPage), navigationParameter);
    }
    #endregion
}