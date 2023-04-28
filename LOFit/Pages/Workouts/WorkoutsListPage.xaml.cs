using LOFit.DataServices.Workout;
using LOFit.Models.Menu;
using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Pages.Coachs;
using LOFit.Pages.Menu;
using LOFit.Tools;
using LOFit.DataServices.Coach;
using LOFit.DataServices.User;

namespace LOFit.Pages.Workouts;

[QueryProperty(nameof(WorkoutDate), "workoutDayDate")]
public partial class WorkoutsListPage : ContentPage
{
    #region Binding prop
    DateTime _workoutDate;
    public DateTime WorkoutDate
    {
        get { return _workoutDate; }
        set
        {
            _workoutDate = value;
            ListLoad();
            OnPropertyChanged();
        }
    }
    #endregion

    private readonly IWorkoutRestService _dataService;
    private readonly ICoachRestService _dataServiceCoach;
    private readonly IUserRestService _dataServiceUser;
    public WorkoutsListPage(IWorkoutRestService dataService, ICoachRestService dataServiceCoach, IUserRestService dataServiceUser)
	{
		InitializeComponent();
        _dataService = dataService;
        _dataServiceCoach = dataServiceCoach;
        _dataServiceUser = dataServiceUser;
    }

    #region Menu buttons
    async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(WorkoutsPage));
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

    #region Lists
    async void ListLoad()
    {
        collectionView.ItemsSource = ListModelTools.ReturnWorkoutList(await _dataService.GetUserList());
    }

    async void OnWorkoutClicked(object sender, SelectionChangedEventArgs e)
    {
        WorkoutListModel modelList = e.CurrentSelection.FirstOrDefault() as WorkoutListModel;
        WorkoutModel model = modelList.Workout;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(WorkoutDayModel), new WorkoutDayModel(){ Data_czas = WorkoutDate, Czas = model.Czas, Kcla= model.Kcla, Opis = model.Opis} },
            { nameof(WorkoutModel), model },
            { "buttonClicked", 2 }
        };

        await Shell.Current.GoToAsync(nameof(WorkoutPage), navigationParameter);
    }

    async void OnDeleteWorkoutClicked(object sender, EventArgs e)
    {

        var button = (Button)sender;
        var id = Int32.Parse(button.CommandParameter.ToString());

        await _dataService.Delete(id);

        ListLoad();

    }
    #endregion
}