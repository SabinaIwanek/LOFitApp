using System.ComponentModel;

namespace LOFit.Models.Accounts
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

        private double _ocena;
        public double Ocena
        {
            get => _ocena;
            set
            {
                if (_ocena == value) return;

                _ocena = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ocena"));
            }
        }
        private string _ocenaString;
        public string OcenaString
        {
            get => _ocenaString;
            set
            {
                if (_ocenaString == value) return;

                _ocenaString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OcenaString"));
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

        private string _dataUr;
        public string DataUr
        {
            get => _dataUr;
            set
            {
                if (_dataUr == value) return;

                _dataUr = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataUr"));
            }
        }
        private string _telefonString;
        public string TelefonString
        {
            get => _telefonString;
            set
            {
                if (_telefonString == value) return;

                _telefonString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TelefonString"));
            }
        }

        public bool ButtonOk { get; set; }
        public bool ButtonNo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
