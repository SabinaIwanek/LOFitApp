using LOFit.Models.Accounts;
using LOFit.Models.Menu;

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

        public static List<MealListModel> ReturnMealList(List<MealModel> list)
        {
            List<MealListModel> newList = new List<MealListModel>();
            if (list == null) return newList;

            foreach (MealModel meal in list)
            {
                MealListModel model = new MealListModel();

                model.Nazwa_dania = newList.Any(x => x.Nazwa_dania == meal.Nazwa_dania) ? "" : meal.Nazwa_dania;
                model.Kcla = (meal.Gramy * meal.Produkt.Kcla) / meal.Produkt.Gramy;

                if (meal.Produkt.Bialko != null) model.Bialko = (meal.Gramy * (int)meal.Produkt.Bialko) / meal.Produkt.Gramy;
                if (meal.Produkt.Tluszcze != null) model.Tluszcze = (meal.Gramy * (int)meal.Produkt.Tluszcze) / meal.Produkt.Gramy;
                if (meal.Produkt.Wegle != null) model.Wegle = (meal.Gramy * (int)meal.Produkt.Wegle) / meal.Produkt.Gramy;

                model.Meal = meal;

                newList.Add(model);
            }

            return newList;
        }
    }
}