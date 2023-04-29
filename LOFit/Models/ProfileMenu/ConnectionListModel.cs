using System.ComponentModel;

namespace LOFit.Models.ProfileMenu
{
    public class ConnectionListModel : INotifyPropertyChanged
    {
        private ConnectionModel _connection;
        public ConnectionModel Connection
        {
            get => _connection;
            set
            {
                if (_connection == value) return;

                _connection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Connection"));
            }
        }

        public string NazwaUser { get; set; }
        public string NazwaTrener { get; set; }
        public string CzasTrwania { get; set; }
        public string PodgladDanych { get; set; }
        public string TelefonUser { get; set; }
        public string Status { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}