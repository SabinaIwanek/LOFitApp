using LOFit.DataServices.Meals;
using LOFit.DataServices.Product;
using LOFit.Models;

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
    public ProductsPage(IProductRestService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
    }
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