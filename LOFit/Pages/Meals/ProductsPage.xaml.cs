using LOFit.DataServices.Coach;
using LOFit.DataServices.User;
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
[QueryProperty(nameof(Model), "Model")]
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
    MealModel _model;
    public MealModel Model
    {
        get { return _model; }
        set
        {
            _model = value;
            OnPropertyChanged();
        }
    }
    #endregion

    private readonly IProductRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;
    public ProductsPage(IProductRestService dataService, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
    }
    #region Menu buttons
    async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MealsPage));
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

    #region Lists
    async void ListLoad()
    {
        if (MyList)
        {
            collectionViewMy.ItemsSource = await _dataService.GetUserList();

            collectionViewMy.IsVisible = true;
            collectionView.IsVisible = false;
        }
        else
        {
            collectionView.ItemsSource = await _dataService.GetAppList();

            collectionViewMy.IsVisible = false;
            collectionView.IsVisible = true;
        }
        
    }

    async void OnProductClicked(object sender, SelectionChangedEventArgs e)
    {
        int button = MyList ? 2 : 3;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(MealModel), Model },
            { nameof(ProductModel), e.CurrentSelection.FirstOrDefault() as ProductModel },
            { "buttonClicked", button as int? }
        };

        await Shell.Current.GoToAsync(nameof(MealPage), navigationParameter);
    }
    async void OnDeleteProductClicked(object sender, EventArgs e)
    {

        var button = (Button)sender;
        var id = Int32.Parse(button.CommandParameter.ToString());

        await _dataService.Delete(id);

        ListLoad();

    }
    #endregion
}