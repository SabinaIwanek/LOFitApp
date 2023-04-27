using System.ComponentModel;

namespace LOFit.Models.Menu
{
    public class WorkoutModel : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int _id_konta;
        public int Id_konta
        {
            get => _id_konta;
            set
            {
                if (_id_konta == value) return;

                _id_konta = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id_konta"));
            }
        }

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

        private DateTime? _czas;
        public DateTime? Czas
        {
            get => _czas;
            set
            {
                if (_czas == value) return;

                _czas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Czas"));
            }
        }

        private int? _kcla;
        public int? Kcla
        {
            get => _kcla;
            set
            {
                if (_kcla == value) return;

                _kcla = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kcla"));
            }
        }

        private string _opis;
        public string Opis
        {
            get => _opis;
            set
            {
                if (_opis == value) return;

                _opis = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Opis"));
            }
        }

        private bool _w_bazie_usera;
        public bool W_bazie_usera
        {
            get => _w_bazie_usera;
            set
            {
                if (_w_bazie_usera == value) return;

                _w_bazie_usera = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("W_bazie_usera"));
            }
        }
        public string CzasString()
        {
            if (Czas == null) return "";

            return $"{Czas.Value.Hour}:{(Czas.Value.Minute < 10 ? "0" : "")}{Czas.Value.Minute}";
        }
        public string OpisString()
        {
            return $" - {Opis}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
