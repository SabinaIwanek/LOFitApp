﻿using LOFit.DataServices.Coach;
using LOFit.DataServices.User;
using LOFit.Models.Accounts;
using LOFit.Models.Menu;
using LOFit.Models.ProfileMenu;

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

                model.Nazwa_dania = newList.LastOrDefault()?.Nazwa_dania == meal.Nazwa_dania ? "" : meal.Nazwa_dania;

                model.Meal = meal;
                model.Kcla = meal.Kcla();
                model.Bialko = meal.Bialko();
                model.Tluszcze = meal.Tluszcze();
                model.Wegle = meal.Wegle();

                newList.Add(model);
            }

            return newList;
        }

        public async static Task<List<OpinionListModel>> ReturnOpinionList(List<OpinionModel> list, IUserRestService dataService)
        {
            List<OpinionListModel> newList = new List<OpinionListModel>();
            if (list == null) return newList;

            foreach (OpinionModel opinion in list)
            {
                OpinionListModel model = new OpinionListModel();

                model.Opinion = opinion;
                model.Imie = await opinion.Imie(dataService);

                newList.Add(model);
            }

            return newList;
        }

        public async static Task<List<ConnectionListModel>> ReturnConnectionList(List<ConnectionModel> list, IUserRestService dataServiceUser, ICoachRestService dataServiceCoach)
        {
            List<ConnectionListModel> newList = new List<ConnectionListModel>();
            if (list == null) return newList;

            foreach (ConnectionModel connection in list)
            {
                ConnectionListModel model = new ConnectionListModel();

                model.Connection = connection;
                model.NazwaUser = await connection.NazwaUser(dataServiceUser);
                model.NazwaTrener = await connection.NazwaTrener(dataServiceCoach);
                model.CzasTrwania = connection.CzasTrwania();
                model.Status = connection.Status();

                newList.Add(model);
            }

            return newList;
        }

        public static List<WorkoutDayListModel> ReturnWorkoutDayList(List<WorkoutDayModel> list)
        {
            List<WorkoutDayListModel> newList = new List<WorkoutDayListModel>();
            if (list == null) return newList;

            foreach (WorkoutDayModel workoutDay in list)
            {
                WorkoutDayListModel model = new WorkoutDayListModel();

                model.WorkoutDay = workoutDay;
                model.CzasString = workoutDay.CzasString();
                model.OpisString = workoutDay.OpisString();

                newList.Add(model);
            }

            return newList;
        }
    }
}