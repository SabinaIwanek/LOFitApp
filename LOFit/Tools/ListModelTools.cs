using LOFit.Models;

namespace LOFit.Tools
{
    public static class ListModelTools
    {
        public static List<CoachListModel> ReturnCoachList(List<CoachModel> list)
        {
            List<CoachListModel> newList= new List<CoachListModel>();
            if(list == null) return newList;

            foreach (CoachModel coach in list)
            {
                CoachListModel model = new CoachListModel();
                
                model.Coach = coach;
                model.Wizytowka = coach.Wizytowka();
                model.Ocena = coach.Ocena();
                model.TypTrenera = coach.TypTrenera();
                model.CenaUslugi = coach.CenaUslugi();

                newList.Add(model);
            }

            return newList;
        }
    }
}