using LOFit.DataServices.Coach;
using LOFit.DataServices.User;
using LOFit.Models.Accounts;
using System.ComponentModel;

namespace LOFit.Models.ProfileMenu
{
    public class ConnectionModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int Id_trenera { get; set; }
        public int Id_usera { get; set; }
        public DateTime Czas_od { get; set; }
        public DateTime? Czas_do { get; set; }
        public int Zatwierdzone { get; set; }

        // Binding
        private DateTime? _podglad_od_daty;
        public DateTime? Podglad_od_daty
        {
            get => _podglad_od_daty;
            set
            {
                if (_podglad_od_daty == value) return;

                _podglad_od_daty = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Podglad_od_daty"));
            }
        }

        public async Task<string> NazwaUser(IUserRestService dataService)
        {
            UserModel model = await dataService.GetOne(Id_usera);
            return $"{model.Imie} {model.Nazwisko}";
        }
        public async Task<string> TelefonUser(IUserRestService dataService)
        {
            UserModel model = await dataService.GetOne(Id_usera);
            return ((int)model.Nr_telefonu).ToString("### ### ###");
        }
        public async Task<string> NazwaTrener(ICoachRestService dataService)
        {
            CoachModel model = await dataService.GetOne(Id_trenera);
            return $"{model.Imie} {model.Nazwisko}";
        }

        public string CzasTrwania()
        {
            string wynik = string.Empty;

            if (Czas_do == null)
                return $"od {Czas_od}";

            return $"od {Czas_od.ToString("dd.MM.yyyy")} do {((DateTime)Czas_do).ToString("dd.MM.yyyy")}";
        }

        public string PodgladDanych()
        {
            if (Podglad_od_daty == null) return "całkowity";

            return $"od dnia {((DateTime)Podglad_od_daty).ToString("dd.MM.yyyy")}";
        }
        public string Status()
        {
            if (Zatwierdzone == 0) return "Nowe";
            if (Zatwierdzone == 1) 
            {
                if(Czas_do == null || Czas_do > DateTime.Now) return "Aktualne";
                else return "Zakończone";
            } 
            if (Zatwierdzone == 2) return "Odrzucone";

            return string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
