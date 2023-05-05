using LOFit.DataServices.Certificate;
using LOFit.Models.ProfileMenu;
using System.ComponentModel;

namespace LOFit.Models.Accounts
{
    public class CoachModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

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

        private string _opis_profilu;
        public string Opis_profilu
        {
            get => _opis_profilu;
            set
            {
                if (_opis_profilu == value) return;

                _opis_profilu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Opis_profilu"));
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

        private decimal? _cena_uslugi;
        public decimal? Cena_uslugi
        {
            get => _cena_uslugi;
            set
            {
                if (_cena_uslugi == value) return;

                _cena_uslugi = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cena_uslugi"));
            }
        }

        private int? _czas_uslugi;
        public int? Czas_uslugi_min
        {
            get => _czas_uslugi;
            set
            {
                if (_czas_uslugi == value) return;

                _czas_uslugi = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Czas_uslugi"));
            }
        }

        public int Zatwierdzony_dietetyk { get; set; } //status weryfikacji
        public int Zatwierdzony_trener { get; set; } //status weryfikacji

        // [ICommand]
        public string Wizytowka()
        {
            return $"{Imie} {Nazwisko}";
        }
        public async Task<(double,string)> Ocena(IOpinionRestService dataService)
        {
            List<OpinionModel> opinie = await dataService.GetCoachList(Id);

            if (!opinie.Any()) return (0, "Brak opinii");

            double ilosc = (double)opinie.Count;
            double srednia = Math.Round(opinie.Sum(x => x.Ocena) / ilosc, 1);


            return (srednia, $" {srednia} ({ilosc} opinie)");
        }

        public string TypTrenera()
        {
            if (Zatwierdzony_dietetyk == 1 && Zatwierdzony_trener == 1)
                return "Dietetyk | Trener";

            if (Zatwierdzony_dietetyk == 1)
                return "Dietetyk";

            if (Zatwierdzony_trener == 1)
                return "Trener";

            return "Brak danych";
        }

        public string CenaUslugi()
        {
            if (Czas_uslugi_min != null && Cena_uslugi != null)
                return $"{Cena_uslugi} zł ({Czas_uslugi_min} min)";

            return "Brak danych";
        }
        public string DataUr()
        {
            if (Data_urodzenia == null)
                return "Brak danych";

            return ((DateTime)Data_urodzenia).ToString("dd.MM.yyyy");
        }
        public string TelefonString()
        {
            if (Nr_telefonu == null) return "";

            return ((int)Nr_telefonu).ToString("### ### ###");
        }
        public string StatusTrenera()
        {
            if (Zatwierdzony_trener == 0) return "W trakcie weryfikacji";
            if (Zatwierdzony_trener == 1) return "Zatwierdzony";
            if (Zatwierdzony_trener == 2) return "Odrzucony";

            return "";
        }
        public string StatusDietetyka()
        {
            if (Zatwierdzony_trener == 0) return "W trakcie weryfikacji";
            if (Zatwierdzony_trener == 1) return "Zatwierdzony";
            if (Zatwierdzony_trener == 2) return "Odrzucony";

            return "";
        }

        public bool ButtonNo() => !(Zatwierdzony_trener == 2 || Zatwierdzony_dietetyk == 2);
        public bool ButtonOk() => !(Zatwierdzony_trener == 1 || Zatwierdzony_dietetyk ==1);

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
