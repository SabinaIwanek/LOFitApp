using System.ComponentModel;

namespace LOFit.Models.Accounts
{
    public class UserModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Imie { get; set; }

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

        private int? _waga_poczatkowa;
        public int? Waga_poczatkowa
        {
            get => _waga_poczatkowa;
            set
            {
                if (_waga_poczatkowa == value) return;

                _waga_poczatkowa = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Waga_poczatkowa"));
            }
        }

        private int? _waga_cel;
        public int? Waga_cel
        {
            get => _waga_cel;
            set
            {
                if (_waga_cel == value) return;

                _waga_cel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Waga_cel"));
            }
        }

        private int? _kcla_dzien;
        public int? Kcla_dzien
        {
            get => _kcla_dzien;
            set
            {
                if (_kcla_dzien == value) return;

                _kcla_dzien = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kcla_dzien"));
            }
        }

        private int? _kcla_dzien_trening;
        public int? Kcla_dzien_trening
        {
            get => _kcla_dzien_trening;
            set
            {
                if (_kcla_dzien_trening == value) return;

                _kcla_dzien_trening = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kcla_dzien_trening"));
            }
        }

        public string Wizytowka()
        {
            return $"{Imie} {Nazwisko}";
        }

        public string DataUr()
        {
            if (Data_urodzenia == null)
                return "Brak danych";

            return ((DateTime)Data_urodzenia).ToString("dd.MM.yyyy");
        }
        public string PlecString()
        {
            Enums.Plec plec = (Enums.Plec)Plec;

            return plec.ToString();
        }
        public string Telefon()
        {
            if (Nr_telefonu == null) return "";

            return ((int)Nr_telefonu).ToString("### ### ###");
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
