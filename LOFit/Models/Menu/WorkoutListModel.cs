using System.ComponentModel;

namespace LOFit.Models.Menu
{
    public class WorkoutListModel : INotifyPropertyChanged
    {
        private WorkoutModel _workout;
        public WorkoutModel Workout
        {
            get => _workout;
            set
            {
                if (_workout == value) return;

                _workout = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Workout"));
            }
        }

        public string CzasString { get; set; }
        public string OpisString { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
