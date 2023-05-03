using LOFit.DataServices.Plan;
using LOFit.DataServices.Coach;
using LOFit.DataServices.User;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;
using LOFit.Pages.Menu;
using LOFit.Pages.Workouts;
using LOFit.Tools;
using LOFit.Models.Menu;
using LOFit.Models.MenuCoach;
using LOFit.DataServices.Workouts;
using LOFit.DataServices.Meals;
using LOFit.Pages.Meals;

namespace LOFit.Pages.MenuCoach;
[QueryProperty(nameof(Typ), "Typ")]
public partial class PlanListPage : ContentPage
{
    private readonly IPlanRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;
    private readonly IWorkoutsRestService _dataServiceWorkouts;
    private readonly IMealRestService _dataServiceMeal;

    #region Binding prop

    private int _type;
    public int Typ
    {
        get { return _type; }
        set
        {
            _type = value;
            ListLoad();

            OnPropertyChanged();
        }
    }
    #endregion
    public PlanListPage(IPlanRestService dataService, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser, IWorkoutsRestService dataServiceWorkouts, IMealRestService dataServiceMeal)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
        _dataServiceWorkouts = dataServiceWorkouts;
        _dataServiceMeal = dataServiceMeal;

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) => OnRightSwiped();

        Content.GestureRecognizers.Add(swipeGestureRight);
        #endregion
    }

    #region Swiped
    async void OnRightSwiped()
    {
        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            if (_type == 0) await Shell.Current.GoToAsync(nameof(WorkoutsPage));
            else if (_type == 1) await Shell.Current.GoToAsync(nameof(MealsPage));
        }
    }
    #endregion

    #region Menu buttons
    void OnBackClicked(object sender, EventArgs e)
    {
        OnRightSwiped();
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

    async void OnPlanClicked(object sender, SelectionChangedEventArgs e)
    {
        if (_type == 0)
        {
            PlanModel plan = e.CurrentSelection.FirstOrDefault() as PlanModel;
            List<List<WorkoutDayModel>> list = await _dataService.GetWorkouts(plan.Id);

            for (int i = 0; i < 7; i++)
            {
                foreach (var item in list[i])
                {
                    WorkoutDayModel model = item;
                    model.Id_planu = 0;
                    model.Id = 0;
                    model.Id_usera = Singleton.Instance.IdUsera;
                    model.Data_czas = Singleton.Instance.DateToShow.AddDays(i);

                    await _dataServiceWorkouts.Add(model);
                }
            }

            OnRightSwiped();
        }
        else if (_type == 1)
        {
            PlanModel plan = e.CurrentSelection.FirstOrDefault() as PlanModel;
            List<List<MealModel>> list = await _dataService.GetMeals(plan.Id);

            for (int i = 0; i < 7; i++)
            {
                foreach (var item in list[i])
                {
                    MealModel model = item;
                    model.Id_planu = 0;
                    model.Id = 0;
                    model.Id_usera = Singleton.Instance.IdUsera;
                    model.Data_czas = Singleton.Instance.DateToShow.AddDays(i);

                    await _dataServiceMeal.Add(model);
                }
            }

            OnRightSwiped();
        }
    }
    #endregion
}