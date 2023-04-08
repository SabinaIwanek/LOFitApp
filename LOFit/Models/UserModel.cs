using System.ComponentModel;

namespace LOFit.Models
{
    public class UserModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Imie { get; set; }

        private string? _nazwisko;
        public string? Nazwisko
        {
            get => _nazwisko;
            set
            {
                if (_nazwisko == value) return;

                _nazwisko = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Nazwisko"));
            }
        }

        private int _plec; //plec
        public int Plec
        {
            get => _plec;
            set
            {
                if (_plec == value) return;

                _plec = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Plec"));
            }
        }

        private DateTime? _data_urodzenia;
        public DateTime? Data_urodzenia
        {
            get => _data_urodzenia;
            set
            {
                if (_data_urodzenia == value) return;

                _data_urodzenia = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Data_urodzenia"));
            }
        }

        public int? Id_trenera { get; set; }
        public int? Id_dietetyka { get; set; }

        private int? _nr_telefonu;
        public int? Nr_telefonu
        {
            get => _nr_telefonu;
            set
            {
                if (_nr_telefonu == value) return;

                _nr_telefonu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Nr_telefonu"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
