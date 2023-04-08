using System.ComponentModel;

namespace LOFit.Models
{
    public class LoginModel : INotifyPropertyChanged
    {
        public LoginModel(bool isCode)
        {
            IsCode= isCode;
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email == value) return;

                _email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;

                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pasword"));
            }
        }

        private int? _code;
        public int? Code
        {
            get => _code;
            set
            {
                if (_code == value) return;

                _code = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Code"));
            }
        }

        private bool _isCode;
        public bool IsCode
        {
            get => _isCode;
            set
            {
                if (_isCode == value) return;

                _isCode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsCode"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}