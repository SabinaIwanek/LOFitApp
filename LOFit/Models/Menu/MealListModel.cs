using System.ComponentModel;

namespace LOFit.Models.Menu
{
    public class MealListModel : INotifyPropertyChanged
    {
        private MealModel _meal;
        public MealModel Meal
        {
            get => _meal;
            set
            {
                if (_meal == value) return;

                _meal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Meal"));
            }
        }

        public bool Nazwa_dania_visible { get; set; }
        public string Nazwa_dania { get; set; }
        public int Kcla { get; set; }
        public int? Bialko { get; set; }
        public int? Tluszcze { get; set; }
        public int? Wegle { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    }
}
