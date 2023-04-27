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

        private decimal? _cena_treningu;
        public decimal? Cena_treningu
        {
            get => _cena_treningu;
            set
            {
                if (_cena_treningu == value) return;

                _cena_treningu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cena_treningu"));
            }
        }

        private TimeOnly? _czas_treningu;
        public TimeOnly? Czas_treningu
        {
            get => _czas_treningu;
            set
            {
                if (_czas_treningu == value) return;

                _czas_treningu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Czas_treningu"));
            }
        }

        private decimal? _cena_dieta;
        public decimal? Cena_dieta
        {
            get => _cena_dieta;
            set
            {
                if (_cena_dieta == value) return;

                _cena_dieta = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cena_dieta"));
            }
        }

        private TimeOnly? _czas_dieta;
        public TimeOnly? Czas_dieta
        {
            get => _czas_dieta;
            set
            {
                if (_czas_dieta == value) return;

                _czas_dieta = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Czas_dieta"));
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
            if (Czas_treningu != null && Cena_treningu != null)
                return $"{Cena_treningu} zł, za {Czas_treningu} minut";

            return "Brak danych";
        }
        public string DataUr()
        {
            if (Data_urodzenia == null)
                return "Brak danych";

            return ((DateTime)Data_urodzenia).ToString("dd.MM.yyyy");
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
