using LOFit.DataServices.Coach;
using LOFit.DataServices.Meals;
using LOFit.DataServices.Product;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.Menu;
using LOFit.Pages.Coachs;
using LOFit.Pages.Menu;
using LOFit.Tools;

namespace LOFit.Pages.Meals;

[QueryProperty(nameof(MyList), "myList")]
[QueryProperty(nameof(MealDate), "mealDate")]
public partial class ProductsPage : ContentPage
{
    #region Binding prop
    bool _myList;
    public bool MyList
    {
        get { return _myList; }
        set
        {
            _myList = value;
            ListLoad();
            OnPropertyChanged();
        }
    }
    DateTime _mealDate;
    public DateTime MealDate
    {
        get { return _mealDate; }
        set
        {
            _mealDate = value;
            OnPropertyChanged();
        }
    }
    #endregion

    private readonly IProductRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    public ProductsPage(IProductRestService dataService, ICoachRestService dataServiceCoach)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
    }
    #region Menu buttons
    async void OnCoachsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CoachsPage));
    }
    void OnChangeThemeClicked(object sender, EventArgs e)
    {
        if (App.Current.UserAppTheme == AppTheme.Dark)
        {
            App.Current.UserAppTheme = AppTheme.Light;
        }
        else
        {
            App.Current.UserAppTheme = AppTheme.Dark;
        }
    }
    async void OnProfileClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
            await Shell.Current.GoToAsync(nameof(ProfilePage));

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

    async void ListLoad()
    {
        collectionView.ItemsSource = MyList? await _dataService.GetUserList() : await _dataService.GetAppList();
    }

    async void OnProductClicked(object sender, SelectionChangedEventArgs e)
    {
        int button = MyList ? 2 : 3;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(MealModel), new MealModel(){ Data_czas = MealDate} },
            { nameof(ProductModel), e.CurrentSelection.FirstOrDefault() as ProductModel },
            { "buttonClicked", button as int? }
        };

        await Shell.Current.GoToAsync(nameof(MealPage), navigationParameter);
    }
}