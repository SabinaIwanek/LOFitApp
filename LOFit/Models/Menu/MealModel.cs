using System.ComponentModel;

namespace LOFit.Models.Menu
{
    public class MealModel : INotifyPropertyChanged
    {
        public MealModel() { }
        public MealModel(MealModel model)
        {
            Id = model.Id;
            Id_produktu = model.Id_produktu;
            Id_usera = model.Id_usera;
            Nazwa_dania = model.Nazwa_dania;
            Gramy = model.Gramy;
            Data_czas = model.Data_czas;
            Opis_od_trenera = model.Opis_od_trenera;
            Id_trenera = model.Id_trenera;
            Zatwierdzony = model.Zatwierdzony;
            Produkt = model.Produkt;
        }

        public int Id { get; set; }

        private int _id_produktu;
        public int Id_produktu
        {
            get => _id_produktu;
            set
            {
                if (_id_produktu == value) return;

                _id_produktu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id_produktu"));
            }
        }

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

        private string _nazwa_dania;
        public string Nazwa_dania
        {
            get => _nazwa_dania;
            set
            {
                if (_nazwa_dania == value) return;

                _nazwa_dania = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Nazwa_dania"));
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

        private DateTime _data_czas;
        public DateTime Data_czas
        {
            get => _data_czas;
            set
            {
                if (_data_czas == value) return;

                _data_czas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Data_czas"));
            }
        }

        private string _opis_od_trenera;
        public string Opis_od_trenera
        {
            get => _opis_od_trenera;
            set
            {
                if (_opis_od_trenera == value) return;

                _opis_od_trenera = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Opis_od_trenera"));
            }
        }

        private int? _id_trenera;
        public int? Id_trenera
        {
            get => _id_trenera;
            set
            {
                if (_id_trenera == value) return;

                _id_trenera = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id_trenera"));
            }
        }

        private bool _zatwierdzony;
        public bool Zatwierdzony
        {
            get => _zatwierdzony;
            set
            {
                if (_zatwierdzony == value) return;

                _zatwierdzony = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Zatwierdzony"));
            }
        }

        public int Kcla()
        {
            return (Gramy * Produkt.Kcla) / Produkt.Gramy;
        }

        public int? Bialko()
        {
            if (Produkt.Bialko == null) return null;

            return (Gramy * (int)Produkt.Bialko) / Produkt.Gramy;
        }

        public int? Tluszcze()
        {
            if (Produkt.Tluszcze == null) return null;

            return (Gramy * (int)Produkt.Tluszcze) / Produkt.Gramy;
        }

        public int? Wegle()
        {
            if (Produkt.Wegle == null) return null;

            return (Gramy * (int)Produkt.Wegle) / Produkt.Gramy;
        }

        public ProductModel Produkt { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
