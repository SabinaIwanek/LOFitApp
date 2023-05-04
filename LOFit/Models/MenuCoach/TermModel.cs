using LOFit.DataServices.Coach;
using LOFit.DataServices.User;
using LOFit.Models.Accounts;
using LOFit.Tools;
using System.ComponentModel;

namespace LOFit.Models.MenuCoach
{
    public class TermModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int Id_trenera { get; set; }
        public int Id_usera { get; set; }

        private DateTime _termin_od;
        public DateTime Termin_od
        {
            get => _termin_od;
            set
            {
                if (_termin_od == value) return;

                _termin_od = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Termin_od"));
            }
        }
        private DateTime _termin_do;
        public DateTime Termin_do
        {
            get => _termin_do;
            set
            {
                if (_termin_do == value) return;

                _termin_do = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Termin_do"));
            }
        }
        public bool Zatwierdzony { get; set; }

        public int MinOd()
        {
            return TermTools.ReturnMinutes(Termin_od);
        }
        public int MinDo()
        {
            return TermTools.ReturnMinutes(Termin_do);
        }
        public async Task<string> NazwaUser(IUserRestService dataService)
        {
            UserModel model = await dataService.GetOne(Id_usera);
            return $"{model.Imie} {model.Nazwisko}";
        }
        public async Task<string> NazwaTrener(ICoachRestService dataService)
        {
            CoachModel model = await dataService.GetOne(Id_trenera);
            return $"{model.Imie} {model.Nazwisko}";
        }
        public string Termin()
        {
            return $"{Termin_od.ToString("HH:mm")} - {Termin_do.ToString("HH:mm")}";
        }
        public string Dzien()
        {
            return Termin_od.ToString("dd-MM-yyyy");
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
