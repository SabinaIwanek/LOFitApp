using System.ComponentModel;

namespace LOFit.Models.Menu
{
    public class WorkoutDayModel : INotifyPropertyChanged
    {
        public WorkoutDayModel() { }
        public WorkoutDayModel(WorkoutDayModel model)
        {
            Id = model.Id;
            Id_usera = model.Id_usera;
            Id_trenera = model.Id_trenera;
            Id_treningu = model.Id_treningu;
            Zatwierdzony = model.Zatwierdzony;
            Trening = model.Trening;
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

        private int _id_treningu;
        public int Id_treningu
        {
            get => _id_treningu;
            set
            {
                if (_id_treningu == value) return;

                _id_treningu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id_treningu"));
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

        public WorkoutModel Trening { get; set; }

        public string CzasString()
        {
            return $"{Czas.Value.Hour}:{(Czas.Value.Minute < 10 ? "0" : "")}{Czas.Value.Minute}";
        }
        public string OpisString()
        {
            return $" - {Opis}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
