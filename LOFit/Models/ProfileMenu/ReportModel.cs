using System.ComponentModel;

namespace LOFit.Models.ProfileMenu
{
    public class ReportModel
    {
        public int Id { get; set; }
        public int Id_trenera { get; set; }
        public int Id_usera { get; set; }

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
        public int Status_weryfikacji { get; set; } //status weryfikacji

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
