using System.ComponentModel;

namespace LOFit.Models.MenuCoach
{
    public class PlanModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int Id_trenera { get; set; }
        public int Typ { get; set; }

        private string _nazwa;
        public string Nazwa
        {
            get => _nazwa;
            set
            {
                if (_nazwa == value) return;

                _nazwa = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Nazwa"));
            }
        }
        public int Dzien1 { get; set; }
        public int Dzien2 { get; set; }
        public int Dzien3 { get; set; }
        public int Dzien4 { get; set; }
        public int Dzien5 { get; set; }
        public int Dzien6 { get; set; }
        public int Dzien7 { get; set; }
        public int Kcla { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
