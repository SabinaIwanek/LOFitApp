using System.ComponentModel;

namespace LOFit.Models
{
    public class CertificateModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int Id_trenera { get; set; }

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

        private string _organizacja;
        public string Organizacja
        {
            get => _organizacja;
            set
            {
                if (_organizacja == value) return;

                _organizacja = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Organizacja"));
            }
        }

        private DateTime _data_certyfikatu;
        public DateTime Data_certyfikatu
        {
            get => _data_certyfikatu;
            set
            {
                if (_data_certyfikatu == value) return;

                _data_certyfikatu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Data_certyfikatu"));
            }
        }

        private string? _kod_certyfikatu;
        public string? Kod_certyfikatu
        {
            get => _kod_certyfikatu;
            set
            {
                if (_kod_certyfikatu == value) return;

                _kod_certyfikatu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kod_certyfikatu"));
            }
        }

        public int Zatwierdzony { get; set; } //status weryfikacji

        public event PropertyChangedEventHandler PropertyChanged;
    }
}