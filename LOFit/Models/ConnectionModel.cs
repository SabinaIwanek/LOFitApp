using System.ComponentModel;

namespace LOFit.Models
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

        public void OK()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
