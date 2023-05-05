using System.ComponentModel;

namespace LOFit.Models.ProfileMenu
{
    public class CertificateListModel : INotifyPropertyChanged
    {
        private CertificateModel _certyfikat;
        public CertificateModel Certyfikat
        {
            get => _certyfikat;
            set
            {
                if (_certyfikat == value) return;

                _certyfikat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Certyfikat"));
            }
        }

        public string DataCert { get; set; }
        public bool ZatwierdzonyBool { get; set; }
        public bool ButtonOk { get; set; }
        public bool ButtonNo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}