using LOFit.DataServices.Coach;
using LOFit.DataServices.User;
using LOFit.DataServices.Meals;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Models.Menu;
using LOFit.Pages.Coachs;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Pages.Workouts;
using LOFit.Tools;
using LOFit.Pages.MenuCoach;
using LOFit.Resources.Styles;

namespace LOFit.Pages.Meals;

public partial class MealsPage : ContentPage
{
    private readonly IMealRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;
    private List<Button> _buttons;

    #region Binding prop
    DateTime _data;
    public DateTime DateCalendar
    {
        get { return _data; }
        set
        {
            if (value.Year == 1900) return;

            _data = value;

            Singleton.Instance.DateToShow = _data;

            OnPropertyChanged();
            ListLoad();
            EntryDate();
        }
    }
    #endregion

    public MealsPage(IMealRestService dataService, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
        BindingContext = this;
        _buttons = new List<Button>() { Button1, Button2, Button3, Button4 };

        if (Singleton.Instance.DateToShow.Year != 1)
        {
            DateCalendar = Singleton.Instance.DateToShow;
        }

        BottomMenuLoad();

        #region Swipe left
        SwipeGestureRecognizer swipeGestureLeft = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Left
        };

        swipeGestureLeft.Swiped += (s, e) => OnLeftSwiped();

        Content.GestureRecognizers.Add(swipeGestureLeft);
        #endregion
    }
    #region Swipe
    async void OnLeftSwiped()
    {
        await Shell.Current.GoToAsync(nameof(MeasurementPage));
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

    #region Date buttons
    void OnButtonDateClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var value = Int32.Parse((string)button.CommandParameter);

        DateCalendar = DateCalendar.AddDays(value);
    }
    void EntryDate()
    {
        _buttons[0].Text = DateCalendar.AddDays(-2).ToString("dd");
        _buttons[1].Text = DateCalendar.AddDays(-1).ToString("dd");
        _buttons[2].Text = DateCalendar.AddDays(1).ToString("dd");
        _buttons[3].Text = DateCalendar.AddDays(2).ToString("dd");
    }
    #endregion

    #region List
    async void ListLoad()
    {
        var list = await _dataService.GetUserList(DateCalendar, Singleton.Instance.IdUsera);
        var user = await _dataServiceUser.GetOne(Singleton.Instance.IdUsera);

        Dispatcher.Dispatch(() =>
        {
            collectionView.ItemsSource = ListModelTools.ReturnMealList(list.Where(x => x.Id_trenera == null).ToList());
            collectionViewCoach.ItemsSource = ListModelTools.ReturnMealList(list.Where(x => x.Id_trenera != null).ToList());

            if (!list.Where(x => x.Id_trenera != null).Any())
            {
                Header1.IsVisible = false;
                Header2.IsVisible = false;
            }
            else
            {
                Header1.IsVisible = true;
                Header2.IsVisible = true;
            }

            if (Singleton.Instance.Type == TypKonta.Trener)
            {
                Header1.IsVisible = true;
            }

            var suma = ((List<MealListModel>)collectionView.ItemsSource)?.Sum(x => x.Kcla) + ((List<MealListModel>)collectionViewCoach.ItemsSource)?.Where(x=>x.Meal.Zatwierdzony).Sum(x => x.Kcla);
            if(user.Kcla_dzien != null)
            {
                ProgresKcla.Progress = (double)suma / (int)user.Kcla_dzien;
                LabelKcla.Text = $"{suma}/{user.Kcla_dzien} Kcla";

                if (suma > user.Kcla_dzien)
                    ProgresKcla.ProgressColor = Colors.Red;
                else
                    ProgresKcla.ProgressColor = MyColors.Primary;
            }
            else
            {
                LabelKcla.Text = $"Suma: {suma} Kcla";
                ProgresKcla.IsVisible = false;
            }
        });
    }
    void OnCheckBoxClicked(object sender, CheckedChangedEventArgs e)
    {
        var check = (CheckBox)sender;
        var model = (MealListModel)check.Parent.BindingContext;
        var id = model.Meal.Id;
        int idChecked = check.IsChecked ? 1 : 0;

        _dataService.CheckedBoxChange(id, idChecked);
    }
    async void OnMealClicked(object sender, SelectionChangedEventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
        {
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
    }
    async void OnDeleteMealClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
        {
            var button = (Button)sender;
            var id = Int32.Parse(button.CommandParameter.ToString());

            await _dataService.Delete(id);

            Dispatcher.Dispatch(() =>
            {
                DateCalendar = DateCalendar;
            });
        }
    }
    async void OnCoachMealClicked(object sender, SelectionChangedEventArgs e)
    {
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

            Dispatcher.Dispatch(() =>
            {
                DateCalendar = DateCalendar;
            });
        }
    }
    #endregion

    #region Bottom buttons
    async void OnAddPlanButtonClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "Typ", 1 }
        };

        await Shell.Current.GoToAsync(nameof(PlanListPage), navigationParameter);
    }
    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(MealModel), new MealModel(){ Data_czas = DateCalendar} },
            { nameof(ProductModel), new ProductModel() },
            { "buttonClicked", 1 }
        };

        await Shell.Current.GoToAsync(nameof(MealPage), navigationParameter);
    }
    void BottomMenuLoad()
    {
        bool isUser = Singleton.Instance.Type == TypKonta.Uzytkownik;

        CoachsBottomButton.IsVisible = isUser;
        ButtonAddBottom.IsVisible = isUser;

        ProfileBottomButton.IsVisible = !isUser;
        ButtonAddCoach.IsVisible = !isUser;
        ButtonPlanTyg.IsVisible = !isUser;

        if (isUser)
        {
            TollbarBack.IconImageSource = "";
            TollbarBack.Text = "";
            TollbarBack.IsEnabled = false;
        }
        else
        {
            TollbarBack.IconImageSource = "back.png";
            TollbarBack.Text = "Back";
            TollbarBack.IsEnabled = true;
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
        }
        else if (parameter == "calendar")
        {
        }
    }
    #endregion
}