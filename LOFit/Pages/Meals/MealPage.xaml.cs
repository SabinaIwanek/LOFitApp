using LOFit.DataServices.Meals;
using LOFit.DataServices.Product;
using LOFit.Models;
using LOFit.Pages.Coachs;
using LOFit.Pages.Menu;
using LOFit.Resources.Styles;
using LOFit.Tools;

namespace LOFit.Pages.Meals;

[QueryProperty(nameof(Model), "MealModel")]
[QueryProperty(nameof(ModelProd), "ProductModel")]
[QueryProperty(nameof(ButtonClicked), "buttonClicked")]
public partial class MealPage : ContentPage
{
    private readonly IMealRestService _dataService;
    private readonly IProductRestService _productDataService;
    private bool _isNew;
    private bool _isNewProd;

    #region Binding prop

    private int _buttonClicked;
    public int ButtonClicked
    {
        get { return _buttonClicked; }
        set
        {
            _buttonClicked = value;
            if (value == 1) OnButtonAddProdClicked(new object(), new EventArgs());
            if (value == 2) MyListClicked();
            if (value == 3) AppListClicked();

            Gramy = Gramy;

            OnPropertyChanged();
        }
    }

    private int _gramy;
    public int Gramy
    {
        get { return _gramy; }
        set
        {
            _gramy = value;
            if (Model != null) Model.Gramy = value;

            if (ButtonClicked > 1)
            {
                Kcla = (value * ModelProd.Kcla) / ModelProd.Gramy;

                if (ModelProd.Bialko != null) Bialko = (value * (int)ModelProd.Bialko) / ModelProd.Gramy;
                if (ModelProd.Tluszcze != null) Tluszcze = (value * (int)ModelProd.Tluszcze) / ModelProd.Gramy;
                if (ModelProd.Wegle != null) Wegle = (value * (int)ModelProd.Wegle) / ModelProd.Gramy;
            }

            OnPropertyChanged();
        }
    }

    private int _kcla;
    public int Kcla
    {
        get { return _kcla; }
        set
        {
            _kcla = value;
            OnPropertyChanged();
        }
    }

    private int _bialko;
    public int Bialko
    {
        get { return _bialko; }
        set
        {
            _bialko = value;
            OnPropertyChanged();
        }
    }

    private int _tluszcze;
    public int Tluszcze
    {
        get { return _tluszcze; }
        set
        {
            _tluszcze = value;
            OnPropertyChanged();
        }
    }

    private int _wegle;
    public int Wegle
    {
        get { return _wegle; }
        set
        {
            _wegle = value;
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
            Gramy = Model.Gramy;

            OnPropertyChanged();
            if(Model.Id_usera == 0)
            {
                _isNew = true;
                Model.Nazwa_dania = DataTools.ReturnDefaultMealName(Model.Data_czas);
            }
            else
            {
                _isNew = false;
            }
        }
    }
    ProductModel _modelProd;
    public ProductModel ModelProd
    {
        get { return _modelProd; }
        set
        {
            _modelProd = value;
            OnPropertyChanged();
            _isNewProd = ModelProd.Id == 0;
        }
    }

    TimeSpan _time;
    public TimeSpan MealTime
    {
        get { return _time; }
        set
        {
            _time = value;
            OnPropertyChanged();
        }
    }
    #endregion

    public MealPage(IMealRestService dataService, IProductRestService productDataService)
    {
        InitializeComponent();
        _dataService = dataService;
        _productDataService = productDataService;
        BindingContext = this;

        MealTime = DateTime.Now.TimeOfDay;

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) =>
        {
            Shell.Current.GoToAsync(nameof(MealsPage));
        };

        Content.GestureRecognizers.Add(swipeGestureRight);
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

    #region Product buttons
    void OnButtonAddProdClicked(object sender, EventArgs e)
    {
        DataTools.ButtonClicked(ButtonAddProd);
        DataTools.ButtonNotClicked(ButtonMyList);
        DataTools.ButtonNotClicked(ButtonAppList);

        IsNewProd(true);
        ModelProd = new ProductModel();
    }
    async void OnButtonMyListClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "myList", true },
            { "mealDate", Model.Data_czas }
        };

        await Shell.Current.GoToAsync(nameof(ProductsPage), navigationParameter);
    }
    async void OnButtonAppListClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "myList", false },
            { "mealDate", Model.Data_czas }
        };

        await Shell.Current.GoToAsync(nameof(ProductsPage), navigationParameter);
    }

    private void IsNewProd(bool isNew)
    {
        EntryNazwa.IsReadOnly = !isNew;
        EntryEan.IsReadOnly = !isNew;
        EntryKcla.IsReadOnly = !isNew;
        EntryBialko.IsReadOnly = !isNew;
        EntryTluszcze.IsReadOnly = !isNew;
        EntryWegle.IsReadOnly = !isNew;

        EntryNazwa.BackgroundColor = isNew ? MyColors.MyEntryBg : MyColors.MyBg;
        EntryEan.BackgroundColor = isNew ? MyColors.MyEntryBg : MyColors.MyBg;

        EntryKcla.IsVisible = isNew;
        EntryBialko.IsVisible = isNew;
        EntryTluszcze.IsVisible = isNew;
        EntryWegle.IsVisible = isNew;

        CalculatedKcla.IsVisible = !isNew;
        CalculatedBialko.IsVisible = !isNew;
        CalculatedTluszcze.IsVisible = !isNew;
        CalculatedWegle.IsVisible = !isNew;
    }
    private void MyListClicked()
    {
        DataTools.ButtonNotClicked(ButtonAddProd);
        DataTools.ButtonClicked(ButtonMyList);
        DataTools.ButtonNotClicked(ButtonAppList);

        IsNewProd(false);
    }
    private void AppListClicked()
    {
        DataTools.ButtonNotClicked(ButtonAddProd);
        DataTools.ButtonNotClicked(ButtonMyList);
        DataTools.ButtonClicked(ButtonAppList);

        IsNewProd(false);
    }
    #endregion

    #region Bottom menu
    async void OnModifyButtonClicked(object sender, EventArgs e)
    {
        if (Model.Nazwa_dania == string.Empty) return;
        if (ModelProd.Nazwa == string.Empty) return;
        if (Model.Gramy == 0) return;
        if (ModelProd.Kcla == 0) return;

        Model.Produkt = ModelProd;
        if (Model.Produkt == null) return;

        string answer;

        if (_isNewProd)
        {
            ModelProd.Gramy = Model.Gramy;
            ModelProd.Id = await _productDataService.Add(ModelProd);
        }
        Model.Id_produktu = ModelProd.Id;

        if (Model.Id_produktu == 0) return;

        Model.Data_czas = new DateTime(Model.Data_czas.Year, Model.Data_czas.Month, Model.Data_czas.Day, MealTime.Hours, MealTime.Minutes, MealTime.Seconds);

        if (_isNew)
            answer = await _dataService.Add(Model);
        else
            answer = await _dataService.Update(Model);

        if (answer == "Ok")
            Shell.Current.GoToAsync(nameof(MealsPage));
    }
    #endregion
}