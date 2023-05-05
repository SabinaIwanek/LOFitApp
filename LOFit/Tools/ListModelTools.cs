using LOFit.DataServices.Admin;
using LOFit.DataServices.Certificate;
using LOFit.DataServices.Coach;
using LOFit.DataServices.User;
using LOFit.Models.Accounts;
using LOFit.Models.Menu;
using LOFit.Models.MenuCoach;
using LOFit.Models.ProfileMenu;

namespace LOFit.Tools
{
    public static class ListModelTools
    {
        public async static Task<List<CoachListModel>> ReturnCoachList(List<CoachModel> list, IOpinionRestService dataService)
        {
            List<CoachListModel> newList = new List<CoachListModel>();
            if (list == null) return newList;

            foreach (CoachModel coach in list)
            {
                CoachListModel model = new CoachListModel();

                model.Coach = coach;
                model.Wizytowka = coach.Wizytowka();
                (double, string) ocena = await coach.Ocena(dataService);
                model.Ocena = ocena.Item1;
                model.OcenaString = ocena.Item2;
                model.TypTrenera = coach.TypTrenera();
                model.CenaUslugi = coach.CenaUslugi();
                model.DataUr = coach.DataUr();
                model.ButtonOk = coach.ButtonOk();
                model.ButtonNo = coach.ButtonNo();

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
                model.PodgladDanych = connection.PodgladDanych();
                model.TelefonUser = await connection.TelefonUser(dataServiceUser);
                model.Status = connection.Status();

                newList.Add(model);
            }

            return newList;
        }
        public async static Task<List<TermListModel>> ReturnTermList(List<TermModel> list, IUserRestService dataServiceUser, ICoachRestService dataServiceCoach)
        {
            List<TermListModel> newList = new List<TermListModel>();
            if (list == null) return newList;

            foreach (TermModel term in list)
            {
                TermListModel model = new TermListModel();

                model.Term = term;
                model.NazwaUser = await term.NazwaUser(dataServiceUser);
                model.NazwaTrener = await term.NazwaTrener(dataServiceCoach);
                model.Termin = term.Termin();
                model.Dzien = term.Dzien();

                newList.Add(model);
            }

            return newList;
        }
        public async static Task<TermListModel> ReturnTermList(TermModel term, IUserRestService dataServiceUser, ICoachRestService dataServiceCoach)
        {
            TermListModel model = new TermListModel();

            model.Term = term;
            model.NazwaUser = await term.NazwaUser(dataServiceUser);
            model.NazwaTrener = await term.NazwaTrener(dataServiceCoach);
            model.Termin = term.Termin();
            model.Dzien = term.Dzien();

            return model;
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
                model.ZgloszonaBool = opinion.ZgloszonaBool();

                newList.Add(model);
            }

            return newList;
        }

        public static List<OpinionListModel> ReturnOpinionList(List<OpinionModel> list)
        {
            List<OpinionListModel> newList = new List<OpinionListModel>();
            if (list == null) return newList;

            foreach (OpinionModel opinion in list)
            {
                OpinionListModel model = new OpinionListModel();

                model.Opinion = opinion;
                model.Imie = "";
                model.ZgloszonaBool = opinion.ZgloszonaBool();
                model.ButtonOk = opinion.ButtonOk();
                model.ButtonNo = opinion.ButtonNo();

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
                model.Nazwa_dania_visible = model.Nazwa_dania != "";
                model.Meal = meal;
                model.Kcla = meal.Kcla();
                model.Bialko = meal.Bialko();
                model.Tluszcze = meal.Tluszcze();
                model.Wegle = meal.Wegle();

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
        public static List<WorkoutListModel> ReturnWorkoutList(List<WorkoutModel> list)
        {
            List<WorkoutListModel> newList = new List<WorkoutListModel>();
            if (list == null) return newList;

            foreach (WorkoutModel workout in list)
            {
                WorkoutListModel model = new WorkoutListModel();

                model.Workout = workout;
                model.CzasString = workout.CzasString();
                model.OpisString = workout.OpisString();

                newList.Add(model);
            }

            return newList;
        }
        public static List<CertificateListModel> ReturnCertificateList(List<CertificateModel> list)
        {
            List<CertificateListModel> newList = new List<CertificateListModel>();
            if (list == null) return newList;

            foreach (CertificateModel certificate in list)
            {
                CertificateListModel model = new CertificateListModel();

                model.Certyfikat = certificate;
                model.DataCert = certificate.DataCert();
                model.ZatwierdzonyBool = certificate.ZatwierdzonyBool();
                model.ButtonOk = certificate.ButtonOk();
                model.ButtonNo = certificate.ButtonNo();

                newList.Add(model);
            }

            return newList;
        }
        public static List<ProductListModel> ReturnProductList(List<ProductModel> list)
        {
            List<ProductListModel> newList = new List<ProductListModel>();
            if (list == null) return newList;

            foreach (ProductModel product in list)
            {
                ProductListModel model = new ProductListModel();

                model.Product = product;
                model.ButtonNo = product.ButtonNo();
                model.ButtonOk = product.ButtonOk();

                newList.Add(model);
            }

            return newList;
        }
        public async static Task<List<UserListModel>> ReturnUserList(List<UserModel> list, IAdminRestService dataService)
        {
            List<UserListModel> newList = new List<UserListModel>();
            if (list == null) return newList;

            foreach (UserModel user in list)
            {
                UserListModel model = new UserListModel();

                model.Id = user.Id;
                model.Imie = user.Imie;
                model.Nazwisko = user.Nazwisko;
                model.ButtonOd = await dataService.IsBlock(user.Id, 1);
                model.ButtonZab = !model.ButtonOd;
                model.ButtonDel = true;

                newList.Add(model);
            }

            return newList;
        }

        public async static Task<List<UserListModel>> ReturnUserList(List<CoachModel> list, IAdminRestService dataService)
        {
            List<UserListModel> newList = new List<UserListModel>();
            if (list == null) return newList;

            foreach (CoachModel user in list)
            {
                UserListModel model = new UserListModel();

                model.Id = user.Id;
                model.Imie = user.Imie;
                model.Nazwisko = user.Nazwisko;
                model.ButtonOd = await dataService.IsBlock(user.Id, 2);
                model.ButtonZab = !model.ButtonOd;
                model.ButtonDel = true;

                newList.Add(model);
            }

            return newList;
        }

        public async static Task<List<UserListModel>> ReturnUserList(List<AdminModel> list, IAdminRestService dataService)
        {
            List<UserListModel> newList = new List<UserListModel>();
            if (list == null) return newList;

            foreach (AdminModel user in list)
            {
                UserListModel model = new UserListModel();

                model.Id = user.Id;
                model.Imie = user.Imie;
                model.Nazwisko = user.Nazwisko;
                model.ButtonOd = false;
                model.ButtonZab = false;
                model.ButtonDel = false;

                newList.Add(model);
            }

            return newList;
        }
    }
}