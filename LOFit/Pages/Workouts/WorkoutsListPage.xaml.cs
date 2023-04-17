using LOFit.DataServices.Workout;
using LOFit.Models.Menu;

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
    public WorkoutsListPage(IWorkoutRestService dataService)
	{
		InitializeComponent();
        _dataService = dataService;
    }
    async void ListLoad()
    {
        collectionView.ItemsSource = await _dataService.GetUserList();
    }

    async void OnWorkoutClicked(object sender, SelectionChangedEventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(WorkoutDayModel), new WorkoutDayModel(){ Data_czas = WorkoutDate} },
            { nameof(WorkoutModel), e.CurrentSelection.FirstOrDefault() as WorkoutModel },
            { "buttonClicked", 2 }
        };

        await Shell.Current.GoToAsync(nameof(WorkoutPage), navigationParameter);
    }
}