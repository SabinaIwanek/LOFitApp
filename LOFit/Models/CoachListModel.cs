using System.ComponentModel;

namespace LOFit.Models
{
    public class CoachListModel : INotifyPropertyChanged
    {
        private CoachModel _coach;
        public CoachModel Coach
        {
            get => _coach;
            set
            {
                if (_coach == value) return;

                _coach = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Coach"));
            }
        }

        private string _wizytowka;
        public string Wizytowka
        {
            get => _wizytowka;
            set
            {
                if (_wizytowka == value) return;

                _wizytowka = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wizytowka"));
            }
        }

        private string _ocena;
        public string Ocena
        {
            get => _ocena;
            set
            {
                if (_ocena == value) return;

                _ocena = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ocena"));
            }
        }

        private string _typTrenera;
        public string TypTrenera
        {
            get => _typTrenera;
            set
            {
                if (_typTrenera == value) return;

                _typTrenera = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TypTrenera"));
            }
        }

        private string _cenaUslugi;
        public string CenaUslugi
        {
            get => _cenaUslugi;
            set
            {
                if (_cenaUslugi == value) return;

                _cenaUslugi = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CenaUslugi"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
