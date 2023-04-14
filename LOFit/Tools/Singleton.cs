using LOFit.Enums;

namespace LOFit.Tools
{
    internal sealed class Singleton
    {
        private static Singleton instance = null;
        private static readonly object padlock = new object();
        public string Token;
        public TypKonta? Type;
        public string User;
        public DateTime DateToShow;
        public int IdTrenera;
        Singleton()
        {
            Token = string.Empty;
            Type = null;
        }

        public static Singleton Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                    return instance;
                }
            }
        }
        public static void Logout()
        {
            Instance.Token = string.Empty;
            Instance.Type = null;
            Instance.User = string.Empty;
        }
    }
}
