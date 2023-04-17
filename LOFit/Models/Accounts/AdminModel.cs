using System.ComponentModel;

namespace LOFit.Models.Accounts
{
    public class AdminModel /*: INotifyPropertyChanged*/
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        /*public event PropertyChangedEventHandler PropertyChanged;*/
    }
}
