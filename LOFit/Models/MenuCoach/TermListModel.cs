using System.ComponentModel;

namespace LOFit.Models.MenuCoach
{
    public class TermListModel : INotifyPropertyChanged
    {
        private TermModel _term;
        public TermModel Term
        {
            get => _term;
            set
            {
                if (_term == value) return;

                _term = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Term"));
            }
        }

        public string NazwaUser { get; set; }
        public string NazwaTrener { get; set; }
        public string Termin { get; set; }
        public string Dzien { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
