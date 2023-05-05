using LOFit.DataServices.User;
using System.ComponentModel;

namespace LOFit.Models.ProfileMenu
{
    public class OpinionModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int Id_usera { get; set; }
        public int Id_trenera { get; set; }

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

        private int _ocena;
        public int Ocena
        {
            get => _ocena;
            set
            {
                if (_ocena == value) return;

                _ocena = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ocena"));
            }
        }

        public int Zgloszona { get; set; } //status weryfikacji

        public async Task<string> Imie(IUserRestService dataService)
        {
            return await dataService.GetName(Id_usera);
        }
        public bool ZgloszonaBool()
        {
            return Zgloszona == 1;
        }

        public bool ButtonNo() => Zgloszona != 3;
        public bool ButtonOk() => Zgloszona != 2;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
