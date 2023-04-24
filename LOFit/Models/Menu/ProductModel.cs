using System.ComponentModel;

namespace LOFit.Models.Menu
{
    public class ProductModel : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int? _id_konta;
        public int? Id_konta
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

        private int? _ean;
        public int? Ean
        {
            get => _ean;
            set
            {
                if (_ean == value) return;

                _ean = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ean"));
            }
        }

        private int _gramy;
        public int Gramy
        {
            get => _gramy;
            set
            {
                if (_gramy == value) return;

                _gramy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Gramy"));
            }
        }

        private int _kcla;
        public int Kcla
        {
            get => _kcla;
            set
            {
                if (_kcla == value) return;

                _kcla = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kcla"));
            }
        }

        private int? _bialko;
        public int? Bialko
        {
            get => _bialko;
            set
            {
                if (_bialko == value) return;

                _bialko = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bialko"));
            }
        }

        private int? _tluszcze;
        public int? Tluszcze
        {
            get => _tluszcze;
            set
            {
                if (_tluszcze == value) return;

                _tluszcze = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tluszcze"));
            }
        }

        private int? _wegle;
        public int? Wegle
        {
            get => _wegle;
            set
            {
                if (_wegle == value) return;

                _wegle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wegle"));
            }
        }

        private int _w_bazie_programu;
        public int W_bazie_programu
        {
            get => _w_bazie_programu;
            set
            {
                if (_w_bazie_programu == value) return;

                _w_bazie_programu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("W_bazie_programu"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
