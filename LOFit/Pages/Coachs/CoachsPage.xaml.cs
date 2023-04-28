using LOFit.DataServices.Certificate;
using LOFit.DataServices.Coach;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Meals;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Pages.Workouts;
using LOFit.Tools;

namespace LOFit.Pages.Coachs;

public partial class CoachsPage : ContentPage
{

    private readonly ICoachRestService _dataService;
    private readonly IOpinionRestService _dataServiceOpinion;
    public CoachsPage(ICoachRestService dataService, IOpinionRestService dataServiceOpinion)
    {
        InitializeComponent();
        _dataService = dataService;
        _dataServiceOpinion = dataServiceOpinion;
        ListLoad();

        #region Swipe right
        SwipeGestureRecognizer swipeGestureRight = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        swipeGestureRight.Swiped += (s, e) => OnSwipeRight();

        Content.GestureRecognizers.Add(swipeGestureRight);
        #endregion
    }

    #region Swiped
    async void OnSwipeRight()
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
        {
            await Shell.Current.GoToAsync(nameof(WorkoutsPage));
        }
    }
    #endregion

    #region Menu buttons
    async void OnProfileClicked(object sender, EventArgs e)
    {
        if (Singleton.Instance.Type == TypKonta.Uzytkownik)
            await Shell.Current.GoToAsync(nameof(ProfilePage));

        if (Singleton.Instance.Type == TypKonta.Trener)
        {
            CoachModel model = await _dataService.GetOne(-1);
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
        List<CoachListModel> myList = new List<CoachListModel>();

        if (Singleton.Instance.Type == TypKonta.Uzytkownik) myList = await ListModelTools.ReturnCoachList(await _dataService.GetMy(1), _dataServiceOpinion);
        List<CoachListModel> list = await ListModelTools.ReturnCoachList(await _dataService.GetAll(), _dataServiceOpinion);

        foreach (var myCoach in myList)
        {
            list.Remove(list.Where(x => x.Coach.Id == myCoach.Coach.Id).First());
        }

        Dispatcher.Dispatch(() =>
        {
            collectionViewMyCoach.ItemsSource = myList;
            collectionView.ItemsSource = list;

            if (myList.Any())
            {
                Header1.IsVisible = true;
                Header2.IsVisible = true;
            }
            else
            {
                Header1.IsVisible = false;
                Header2.IsVisible = false;
            }
        });
    }

    async void OnCoachClicked(object sender, SelectionChangedEventArgs e)
    {
        CoachListModel model = e.CurrentSelection.FirstOrDefault() as CoachListModel;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CoachModel), model.Coach as CoachModel}
        };

        await Shell.Current.GoToAsync(nameof(CoachPage), navigationParameter);
    }
    #endregion

    #region Bottom menu
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
    }
    #endregion
}