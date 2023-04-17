using System.ComponentModel;

namespace LOFit.Models.Accounts
{
    public class RegistrationModel : INotifyPropertyChanged
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email == value) return;

                _email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
            }
        }
        private string _haslo;
        public string Haslo
        {
            get => _haslo;
            set
            {
                if (_haslo == value) return;

                _haslo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Haslo"));
            }
        }
        private string _imie;
        public string Imie
        {
            get => _imie;
            set
            {
                if (_imie == value) return;

                _imie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Imie"));
            }
        }
        private string _nazwisko;
        public string Nazwisko
        {
            get => _nazwisko;
            set
            {
                if (_nazwisko == value) return;

                _nazwisko = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Nazwisko"));
            }
        }
        private int _plec;
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
        private string _miejscowosc;
        public string Miejscowosc
        {
            get => _miejscowosc;
            set
            {
                if (_miejscowosc == value) return;

                _miejscowosc = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Miejscowosc"));
            }
        }
        private int _typ_trenera;
        public int Typ_trenera
        {
            get => _typ_trenera;
            set
            {
                if (_typ_trenera == value) return;

                _typ_trenera = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Typ_trenera"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
