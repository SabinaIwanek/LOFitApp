using LOFit.DataServices.Coach;
using LOFit.Enums;
using LOFit.Models;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Tools;

namespace LOFit.Pages.Coachs;

public partial class CoachsPage : ContentPage
{

    private readonly ICoachRestService _dataService;
    public CoachsPage(ICoachRestService dataService)
	{
		InitializeComponent();
        _dataService = dataService;
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
            await Shell.Current.GoToAsync(nameof(MeasurementPage));
        }
    }
    /*async void OnSwipeLeft()
    {

    }*/
    #endregion

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

    #region Lists
    async void ListLoad()
    {
        List<CoachListModel> myList = ListModelTools.ReturnCoachList(await _dataService.GetMy(1));
        List<CoachListModel> list = ListModelTools.ReturnCoachList(await _dataService.GetAll());

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
}