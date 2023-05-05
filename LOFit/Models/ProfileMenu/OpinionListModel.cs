using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOFit.Models.ProfileMenu
{
    public class OpinionListModel : INotifyPropertyChanged
    {
        private OpinionModel _opinion;
        public OpinionModel Opinion
        {
            get => _opinion;
            set
            {
                if (_opinion == value) return;

                _opinion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Opinion"));
            }
        }

        public string Imie { get; set; }
        public bool ZgloszonaBool { get; set; }
        public bool ButtonOk { get; set; }
        public bool ButtonNo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}