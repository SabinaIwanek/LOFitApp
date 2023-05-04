using LOFit.Models.MenuCoach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOFit.Tools
{
    public static class TermTools
    {
        public static bool ChechNewTerm(List<TermModel> list, TimeSpan time_od, TimeSpan time_do)
        {
            if (list == null || !list.Any()) return true;

            var sortList = list.OrderBy(x=>x.MinOd()).ToArray();
            int min_od = ReturnMinutes(time_od);
            int min_do = ReturnMinutes(time_do);

            foreach (var item in sortList)
            {
                if (item.MinDo() <= min_od) continue;
                else if (item.MinOd() >= min_do) return true;

                return false;
            }

            return true;
        }
        public static int ReturnMinutes(TimeSpan time)
        {
            return 60  * time.Hours + time.Minutes;
        }
        public static int ReturnMinutes(DateTime time)
        {
            return 60  * time.Hour + time.Minute;
        }
        public static string ReturnWeekDay(DateTime time)
        {
            var week = time.DayOfWeek;

            if (week == DayOfWeek.Monday) return "Poniedziałek";
            if (week == DayOfWeek.Tuesday) return "Wtorek";
            if (week == DayOfWeek.Wednesday) return "Środa";
            if (week == DayOfWeek.Thursday) return "Czwartek";
            if (week == DayOfWeek.Friday) return "Piątek";
            if (week == DayOfWeek.Saturday) return "Sobota";
            if (week == DayOfWeek.Sunday) return "Niedziela";

            return "Day";
        }
    }
}
