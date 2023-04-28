using LOFit.DataServices.Coach;
using LOFit.DataServices.Meals;
using LOFit.DataServices.Product;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.Menu;
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
    private readonly ICoachRestService _dataServiceCoach;
    private List<Button> _buttons;
    private List<Grid> _grids;
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
            if (Model.Id_usera == 0)
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

    public MealPage(IMealRestService dataService, IProductRestService productDataService, ICoachRestService dataServiceCoach)
    {
        InitializeComponent();
        _dataService = dataService;
        _productDataService = productDataService;
        _dataServiceCoach = dataServiceCoach;
        BindingContext = this;
        _buttons = new List<Button>() { ButtonAddProd, ButtonMyList, ButtonAppList };
        _grids = new List<Grid>() { BottomAddProd, BottomMyList, BottomAppList };

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
    async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MealsPage));
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

    #region Product buttons
    void OnButtonAddProdClicked(object sender, EventArgs e)
    {
        DataTools.ButtonNotClicked(_buttons, _grids);
        DataTools.ButtonClicked(ButtonAddProd, BottomAddProd);

        ButtonClicked = 0;
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
        checkBoxGrid.IsVisible = isNew;

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
        DataTools.ButtonNotClicked(_buttons, _grids);
        DataTools.ButtonClicked(ButtonMyList, BottomMyList);

        IsNewProd(false);
    }
    private void AppListClicked()
    {
        DataTools.ButtonNotClicked(_buttons, _grids);
        DataTools.ButtonClicked(ButtonAppList, BottomAppList);

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
            if (checkBox.IsChecked) ModelProd.Id_konta = -1;
            else ModelProd.Id_konta = 0;

            ModelProd.Gramy = Model.Gramy;
            ModelProd.Id = await _productDataService.Add(ModelProd);
        }
        Model.Id_produktu = ModelProd.Id;

        if (Model.Id_produktu == 0) return;

        Model.Data_czas = new DateTime(Model.Data_czas.Year, Model.Data_czas.Month, Model.Data_czas.Day, MealTime.Hours, MealTime.Minutes, MealTime.Seconds);
        Model.Id_usera = Singleton.Instance.IdUsera;

        if (_isNew)
        {
            if (Singleton.Instance.Type == TypKonta.Trener)
                Model.Id_trenera = Singleton.Instance.IdTrenera;

            answer = await _dataService.Add(Model);
        }
        else
            answer = await _dataService.Update(Model);

        if (answer == "Ok")
            await Shell.Current.GoToAsync(nameof(MealsPage));
    }
    #endregion
}