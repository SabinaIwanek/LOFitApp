using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOFit.Models.Menu
{
    public class WorkoutDayListModel : INotifyPropertyChanged
    {
        private WorkoutDayModel _workoutDay;
        public WorkoutDayModel WorkoutDay
        {
            get => _workoutDay;
            set
            {
                if (_workoutDay == value) return;

                _workoutDay = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WorkoutDay"));
            }
        }

        public string CzasString { get; set; }
        public string OpisString { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}