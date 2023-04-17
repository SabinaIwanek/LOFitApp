using System.ComponentModel;

namespace LOFit.Models.Menu
{
    public class MeasurementModel : INotifyPropertyChanged
    {
        public MeasurementModel() { }
        public MeasurementModel(MeasurementModel model)
        {
            Id = model.Id;
            Id_usera = model.Id_usera;
            Data_pomiaru = model.Data_pomiaru;
            Waga = model.Waga;
            Procent_tluszczu = model.Procent_tluszczu;
            Biceps = model.Biceps;
            Klatka_piersiowa = model.Klatka_piersiowa;
            Pod_klatka_piersiowa = model.Pod_klatka_piersiowa;
            Talia = model.Talia;
            Pas = model.Pas;
            Posladki = model.Posladki;
            Udo = model.Udo;
            Kolano = model.Kolano;
            Lydka = model.Lydka;
        }

        public int Id { get; set; }

        private int _id_usera;
        public int Id_usera
        {
            get => _id_usera;
            set
            {
                if (_id_usera == value) return;

                _id_usera = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id_usera"));
            }
        }

        private DateTime _data_pomiaru;
        public DateTime Data_pomiaru
        {
            get => _data_pomiaru;
            set
            {
                if (_data_pomiaru == value) return;

                _data_pomiaru = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Data_pomiaru"));
            }
        }

        private decimal? _waga;
        public decimal? Waga
        {
            get => _waga;
            set
            {
                if (_waga == value) return;

                _waga = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Waga"));
            }
        }

        private decimal? _procent_tluszczu;
        public decimal? Procent_tluszczu
        {
            get => _procent_tluszczu;
            set
            {
                if (_procent_tluszczu == value) return;

                _procent_tluszczu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Procent_tluszczu"));
            }
        }

        private decimal? _biceps;
        public decimal? Biceps
        {
            get => _biceps;
            set
            {
                if (_biceps == value) return;

                _biceps = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Biceps"));
            }
        }

        private decimal? _klatka_piersiowa;
        public decimal? Klatka_piersiowa
        {
            get => _klatka_piersiowa;
            set
            {
                if (_klatka_piersiowa == value) return;

                _klatka_piersiowa = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Klatka_piersiowa"));
            }
        }

        private decimal? _pod_klatka_piersiowa;
        public decimal? Pod_klatka_piersiowa
        {
            get => _pod_klatka_piersiowa;
            set
            {
                if (_pod_klatka_piersiowa == value) return;

                _pod_klatka_piersiowa = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pod_klatka_piersiowa"));
            }
        }

        private decimal? _talia;
        public decimal? Talia
        {
            get => _talia;
            set
            {
                if (_talia == value) return;

                _talia = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Talia"));
            }
        }

        private decimal? _pas;
        public decimal? Pas
        {
            get => _pas;
            set
            {
                if (_pas == value) return;

                _pas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pas"));
            }
        }

        private decimal? _posladki;
        public decimal? Posladki
        {
            get => _posladki;
            set
            {
                if (_posladki == value) return;

                _posladki = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Posladki"));
            }
        }

        private decimal? _udo;
        public decimal? Udo
        {
            get => _udo;
            set
            {
                if (_udo == value) return;

                _udo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Udo"));
            }
        }

        private decimal? _kolano;
        public decimal? Kolano
        {
            get => _kolano;
            set
            {
                if (_kolano == value) return;

                _kolano = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kolano"));
            }
        }

        private decimal? _lydka;
        public decimal? Lydka
        {
            get => _lydka;
            set
            {
                if (_lydka == value) return;

                _lydka = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Lydka"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
