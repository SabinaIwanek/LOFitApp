using System.ComponentModel;

namespace LOFit.Models.MenuCoach
{
    public class TermModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int Id_trenera { get; set; }
        public int Id_usera { get; set; }

        private DateTime _termin_od;
        public DateTime Termin_od
        {
            get => _termin_od;
            set
            {
                if (_termin_od == value) return;

                _termin_od = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Termin_od"));
            }
        }
        private DateTime _termin_do;
        public DateTime Termin_do
        {
            get => _termin_do;
            set
            {
                if (_termin_do == value) return;

                _termin_do = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Termin_do"));
            }
        }
        public bool Zatwierdzony { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
