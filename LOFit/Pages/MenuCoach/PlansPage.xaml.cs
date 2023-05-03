using LOFit.DataServices.Plan;
using LOFit.DataServices.Coach;
using LOFit.DataServices.User;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Pages.Workouts;
using LOFit.Tools;
using LOFit.Pages.Meals;
using LOFit.Models.Menu;
using LOFit.Models.MenuCoach;

namespace LOFit.Pages.MenuCoach;

public partial class PlansPage : ContentPage
{
    private readonly IPlanRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;
    private List<Button> _buttons;
    private List<Grid> _grids;
    private int _type;
    public PlansPage(IPlanRestService dataService, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
        _buttons = new List<Button>() { WorkoutsButton, MealsButton };
        _grids = new List<Grid>() { BottomWorkoutsButton, BottomMealsButton };
        _type = 0;
        Singleton.Instance.PlanDay = 0;
        ListLoad();

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) => OnSwipeRight();

        Content.GestureRecognizers.Add(swipeGestureRight);
        #endregion

        #region Swipe Left
        SwipeGestureRecognizer swipeGestureLeft = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Left
        };

        swipeGestureLeft.Swiped += (s, e) => OnSwipeLeft();

        Content.GestureRecognizers.Add(swipeGestureLeft);
        #endregion
    }

    #region Swiped
    async void OnSwipeRight()
    {

    }
    async void OnSwipeLeft()
    {
        await Shell.Current.GoToAsync(nameof(ConnectionsPage));
    }
    #endregion

    #region Menu buttons
    async void OnBackClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            await Shell.Current.GoToAsync(nameof(ConnectionsPage));
        }
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

    #region List
    async void ListLoad()
    {
        collectionViewCoach.ItemsSource = await _dataService.GetByType(_type);
    }
    void OnListTypeClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        _type = Int32.Parse((string)button.CommandParameter);

        ListLoad();

        DataTools.ButtonNotClicked(_buttons, _grids);
        DataTools.ButtonClicked(_buttons[_type], _grids[_type]);

    }
    async void OnPlanClicked(object sender, SelectionChangedEventArgs e)
    {
        if(_type == 0)
        {
            PlanModel plan = e.CurrentSelection.FirstOrDefault() as PlanModel;
            List<List<WorkoutDayModel>> list = await _dataService.GetWorkouts(plan.Id);

            var navigationParameter = new Dictionary<string, object>
            {
                { "WorkoutsList", list },
                { "Plan", plan }
            };

            await Shell.Current.GoToAsync(nameof(PlanWorkoutPage), navigationParameter);
        }
        if (_type == 1)
        {
            PlanModel plan = e.CurrentSelection.FirstOrDefault() as PlanModel;
            List<List<MealModel>> list = await _dataService.GetMeals(plan.Id);

            var navigationParameter = new Dictionary<string, object>
            {
                { "MealsList", list },
                { "Plan", plan }
            };

            await Shell.Current.GoToAsync(nameof(PlanMealPage), navigationParameter);
        }
    }
    async void OnDeletePlanClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            var button = (Button)sender;
            var id = Int32.Parse(button.CommandParameter.ToString());

            await _dataService.Delete(id);

            ListLoad();
        }
    }
    #endregion

    #region Bottom buttons
    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (_type == 0)
        {
            List<List<WorkoutDayModel>> list = new List<List<WorkoutDayModel>>();
            for (int i = 0; i < 7; i++)
            {
                list.Add(new List<WorkoutDayModel>());
            }

            int planId = await _dataService.Add(new PlanModel { Typ = 0, Nazwa = "" });
            PlanModel plan = await _dataService.GetOne(planId);

            var navigationParameter = new Dictionary<string, object>
            {
                { "WorkoutsList", list },
                { "Plan", plan }
            };

            await Shell.Current.GoToAsync(nameof(PlanWorkoutPage), navigationParameter);
        }
        if (_type == 1)
        {
            List<List<MealModel>> list = new List<List<MealModel>>();
            for (int i = 0; i < 7; i++)
            {
                list.Add(new List<MealModel>());
            }

            int planId = await _dataService.Add(new PlanModel { Typ = 1, Nazwa = "" });
            PlanModel plan = await _dataService.GetOne(planId);

            var navigationParameter = new Dictionary<string, object>
            {
                { "MealsList", list },
                { "Plan", plan }
            };

            await Shell.Current.GoToAsync(nameof(PlanMealPage), navigationParameter);
        }
    }
    async void OnBottomMenuClicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        var parameter = (string)button.CommandParameter;

        if (parameter == "meals")
        {
            await Shell.Current.GoToAsync(nameof(MealsPage));
        }
        else if (parameter == "measure")
        {
            await Shell.Current.GoToAsync(nameof(MeasurementPage));
        }
        else if (parameter == "workout")
        {
            await Shell.Current.GoToAsync(nameof(WorkoutsPage));
        }
        else if (parameter == "coachs")
        {
            await Shell.Current.GoToAsync(nameof(CoachsPage));
        }
        else if (parameter == "coachsProfile")
        {
            OnProfileClicked(sender, e);
        }
        else if (parameter == "userProfile")
        {
            UserModel user = await _dataServiceUser.GetOne(Singleton.Instance.IdUsera);

            var navigationParameter = new Dictionary<string, object>
                {
                    { nameof(UserModel), user}
                };

            await Shell.Current.GoToAsync(nameof(ProfilePage), navigationParameter);
        }
        else if (parameter == "connections")
        {
            if (Singleton.Instance.Type == TypKonta.Trener)
                await Shell.Current.GoToAsync(nameof(ConnectionsPage));
        }
        else if (parameter == "plans")
        {
            if (Singleton.Instance.Type == TypKonta.Trener)
                await Shell.Current.GoToAsync(nameof(PlansPage));
        }
        else if (parameter == "calendar")
        {
        }
    }
    #endregion
}