using LOFit.DataServices.Coach;
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

        swipeGestureRight.Swiped += (s, e) =>
        {
            Shell.Current.GoToAsync(nameof(MeasurementPage));
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
    async void ListLoad()
    {
        collectionView.ItemsSource = ListModelTools.ReturnCoachList(await _dataService.GetAll());
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
}