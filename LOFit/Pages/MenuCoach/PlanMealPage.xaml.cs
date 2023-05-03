using LOFit.DataServices.Coach;
using LOFit.DataServices.Meals;
using LOFit.DataServices.Measurement;
using LOFit.DataServices.Plan;
using LOFit.DataServices.User;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.Menu;
using LOFit.Models.MenuCoach;
using LOFit.Pages.Coachs;
using LOFit.Pages.Meals;
using LOFit.Pages.Menu;
using LOFit.Pages.Workouts;
using LOFit.Tools;

namespace LOFit.Pages.MenuCoach;

[QueryProperty(nameof(MealsList), "MealsList")]
[QueryProperty(nameof(Plan), "Plan")]
public partial class PlanMealPage : ContentPage
{
    private readonly IMealRestService _dataService;
    private readonly IMeasurementRestService _dataServiceMeasurement;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;
    private readonly IPlanRestService _dataServicePlan;
    private List<Button> _buttons;
    private int _day;

    #region Binding prop

    List<List<MealModel>> _list;
    public List<List<MealModel>> MealsList
    {
        get { return _list; }
        set
        {
            _list = value;
            ListLoad();

            OnPropertyChanged();
        }
    }

    PlanModel _plan;
    public PlanModel Plan
    {
        get { return _plan; }
        set
        {
            _plan = value;
            ListLoad();

            OnPropertyChanged();
        }
    }
    #endregion

    public PlanMealPage(IMealRestService dataService, IMeasurementRestService dataServiceMeasurement, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser, IPlanRestService dataServicePlan)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceMeasurement = dataServiceMeasurement;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
        _dataServicePlan = dataServicePlan;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3, Button4 };

        if (Singleton.Instance.PlanDay == 0)
            EntryDate(1);
        else
            EntryDate(Singleton.Instance.PlanDay);

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) => OnRightSwiped();

        Content.GestureRecognizers.Add(swipeGestureRight);
        #endregion
    }

    #region Swipe
    async void OnRightSwiped()
    {
        await _dataServicePlan.Update(Plan);
        await Shell.Current.GoToAsync(nameof(PlansPage));
    }
    #endregion

    #region Menu buttons
    async void OnBackClicked(object sender, EventArgs e)
    {
        await _dataServicePlan.Update(Plan);
        await Shell.Current.GoToAsync(nameof(PlansPage));
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

    #region Date buttons
    void OnButtonDateClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var value = Int32.Parse((string)button.CommandParameter);

        var day = _day + value;
        if (day > 0 && day < 8)
        {
            EntryDate(day);
            ListLoad();
        }
    }
    void EntryDate(int day)
    {
        _day = day;
        Singleton.Instance.PlanDay = _day;

        _buttons[0].Text = _day > 2 ? $"{day - 2}" : "";
        _buttons[1].Text = _day > 1 ? $"{day - 1}" : "";
        LabelDay.Text = $"Dzieñ {day}";
        _buttons[2].Text = _day < 7 ? $"{day + 1}" : "";
        _buttons[3].Text = _day < 6 ? $"{day + 2}" : "";
    }
    #endregion

    #region List
    void ListLoad()
    {
        collectionViewCoach.ItemsSource = ListModelTools.ReturnMealList(_list[_day - 1]);
    }

    async void OnCoachMealClicked(object sender, SelectionChangedEventArgs e)
    {
        await _dataServicePlan.Update(Plan);

        MealListModel listModel = e.CurrentSelection.FirstOrDefault() as MealListModel;
        MealModel meal = listModel.Meal;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(MealModel), meal },
            { nameof(ProductModel), meal.Produkt},
            { "buttonClicked", 2 }
        };

        await Shell.Current.GoToAsync(nameof(MealPage), navigationParameter);
    }
    async void OnDeleteCoachMealClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            var button = (Button)sender;
            var id = Int32.Parse(button.CommandParameter.ToString());

            await _dataService.Delete(id);

            _list = await _dataServicePlan.GetMeals(Plan.Id);
            ListLoad();
        }
    }
    #endregion

    #region Bottom Button
    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        await _dataServicePlan.Update(Plan);

        int id = Int32.Parse($"{_day}{Plan.Id}");

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(MealModel), new MealModel(){ Id_planu = id, Data_czas = DateTime.Now} },
            { nameof(ProductModel), new ProductModel() },
            { "buttonClicked", 1 }
        };

        await Shell.Current.GoToAsync(nameof(MealPage), navigationParameter);
    }
    #endregion
}