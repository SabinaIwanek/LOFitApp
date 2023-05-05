using LOFit.DataServices.Admin;
using LOFit.Models.Accounts;
using LOFit.Pages.Admin.VerifyLists;
using LOFit.Tools;

namespace LOFit.Pages.Admin.VerifyLists;

public partial class VerifyProductsPage : ContentPage
{
    private readonly IAdminRestService _dataService;

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

    private int _infoVerifyReport;
    public int InfoVerifyReport
    {
        get { return _infoVerifyReport; }
        set
        {
            _infoVerifyReport = value;
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

    public VerifyProductsPage(IAdminRestService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        BindingContext = this;

        OnLoadData();
    }

    #region Load data
    async void OnLoadData()
    {
        AdminModel admin = await _dataService.GetOne(-1);
        AdminName = $"{admin.Imie} {admin.Nazwisko}";

        InfoCoachs = (await _dataService.GetWgTypeCoach(0)).Count;
        InfoCertificate = (await _dataService.GetWgTypeCert(0)).Count;
        InfoVerifyOpinion = (await _dataService.GetWgTypeOpinion(0)).Count;
        InfoVerifyReport = (await _dataService.GetWgTypeReports(0)).Count;
        InfoProducts = (await _dataService.GetWgTypeProducts(0)).Count;

        InfoCoach.IsVisible = InfoCoachs != 0;
        InfoCert.IsVisible = InfoCertificate != 0;
        InfoOpinion.IsVisible = InfoVerifyOpinion != 0;
        InfoReport.IsVisible = InfoVerifyReport != 0;
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
        else if (parameter == "reports")
        {
            await Shell.Current.GoToAsync(nameof(VerifyReportPage));
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
}