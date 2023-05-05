namespace LOFit.Models.Accounts
{
    public class UserListModel
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public bool ButtonZab { get; set; }
        public bool ButtonOd { get; set; }
        public bool ButtonDel { get; set; }
    }
}