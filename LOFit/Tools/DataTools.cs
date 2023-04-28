using LOFit.Enums;
using LOFit.Resources.Styles;
using System.Globalization;

namespace LOFit.Tools
{
    public static class DataTools
    {
        //Check
        public static bool CheckEmail(string email)
        {
            if (email == string.Empty) return false;

            try
            {
                string[] spaces = email.Split(' ');
                if (spaces.Count() > 1) return false;

                string[] monkey = email.Split('@');
                if (monkey.Count() != 2) return false;
            }
            catch { return false; }

            return true;
        }

        //Return
        public static int EntryGender(string gender)
        {
            if (gender == Plec.Kobieta.ToString()) return (int)Plec.Kobieta;
            if (gender == Plec.Mężczyzna.ToString()) return (int)Plec.Mężczyzna;

            return (int)Plec.NiePodano;
        }
        public static string ReturnGender(int gender)
        {
            if (gender == 0) return "Nie podano";

            return ((Plec)gender).ToString();
        }
        public static int EntryCoach(string coach)
        {
            if (coach == TypTrenera.Trener.ToString()) return (int)TypTrenera.Trener;
            if (coach == TypTrenera.Dietetyk.ToString()) return (int)TypTrenera.Dietetyk;

            return (int)TypTrenera.Oba;
        }
        public static DateTime ReturnFirstDayWeek(DateTime date)
        {
            CultureInfo ci = new CultureInfo("pl-PL");
            ci.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;

            DateTimeFormatInfo dtfi = ci.DateTimeFormat;
            Calendar cal = ci.Calendar;

            int currentDayOfWeek = (int)cal.GetDayOfWeek(date);
            int daysToSubtract = (currentDayOfWeek == 0) ? 6 : currentDayOfWeek - 1;
            DateTime firstDayWeek = date.AddDays(-daysToSubtract);

            return firstDayWeek;
        }
        public static string ReturnDefaultMealName(DateTime date)
        {
            date = DateTime.Now;
            if (date.Hour < 6) return "Nocne posiłki";
            if (date.Hour < 10) return "Śniadanie";
            if (date.Hour < 12) return "Drugie śniadanie";
            if (date.Hour < 15) return "Obiad";
            if (date.Hour < 17) return "Podwieczorek";
            if (date.Hour < 21) return "Kolacja";

            return "Nocne posiłki";
        }

        //Actions
        public static int ButtonClicked(DayOfWeek dayOfWeek, List<Button> buttons)
        {
            ButtonNotClicked(buttons);

            if (dayOfWeek == DayOfWeek.Monday)
            {
                ButtonClicked(buttons[0]);
                return 0;
            }
            if (dayOfWeek == DayOfWeek.Tuesday)
            {
                ButtonClicked(buttons[1]);
                return 1;
            }
            if (dayOfWeek == DayOfWeek.Wednesday)
            {
                ButtonClicked(buttons[2]);
                return 2;
            }
            if (dayOfWeek == DayOfWeek.Thursday)
            {
                ButtonClicked(buttons[3]);
                return 3;
            }
            if (dayOfWeek == DayOfWeek.Friday)
            {
                ButtonClicked(buttons[4]);
                return 4;
            }
            if (dayOfWeek == DayOfWeek.Saturday)
            {
                ButtonClicked(buttons[5]);
                return 5;
            }
            if (dayOfWeek == DayOfWeek.Sunday)
            {
                ButtonClicked(buttons[6]);
                return 6;
            }

            return 0;
        }
        public static void ButtonClicked(Button button)
        {
            button.Background = MyColors.Primary;
        }
        public static void ButtonClicked(Button button, Grid grid)
        {
            button.TextColor = MyColors.Primary;
            grid.BackgroundColor = MyColors.Primary;
        }
        public static void ButtonNotClicked(Button button)
        {
            button.Background = MyColors.MyText;
        }
        public static void ButtonNotClicked(List<Button> buttons)
        {
            foreach (var button in buttons)
            {
                button.Background = MyColors.MyText;
            }
        }
        public static void ButtonNotClicked(Button button, Grid grid)
        {
            button.TextColor = MyColors.MyText;

            grid.BackgroundColor = Colors.Transparent;
        }
        public static void ButtonNotClicked(List<Button> buttons, List<Grid> grids)
        {
            foreach (var button in buttons)
            {
                button.TextColor = MyColors.MyText;
            }

            foreach (var grid in grids)
            {
                grid.BackgroundColor = Colors.Transparent;
            }
        }
        public static void StarsImage(List<Image> stars, double rate)
        {
            foreach (var star in stars)
            {
                star.Source = "star_empty.png";
            }

            if(rate >= 4.5 ) stars[4].Source = "star.png";
            if(rate >= 3.5 ) stars[3].Source = "star.png";
            if(rate >= 2.5 ) stars[2].Source = "star.png";
            if(rate >= 1.5 ) stars[1].Source = "star.png";
            if(rate != 0 ) stars[0].Source = "star.png";
        }
        public static void StarsImageButton(List<ImageButton> stars, int rate)
        {
            foreach (var star in stars)
            {
                star.Source = "star_empty.png";
            }

            if (rate > 4) stars[4].Source = "star.png";
            if (rate > 3) stars[3].Source = "star.png";
            if (rate > 2) stars[2].Source = "star.png";
            if (rate > 1) stars[1].Source = "star.png";
            stars[0].Source = "star.png";
        }
    }
}
