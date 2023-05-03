using LOFit.Pages.Menu;
using LOFit.Pages.Coachs;
using LOFit.Pages.Login;
using LOFit.Pages.Measures;
using LOFit.Pages.Meals;
using LOFit.Pages.Workouts;
using LOFit.Pages.Admin;
using LOFit.Pages.Admin.VerifyLists;
using LOFit.Pages.MenuCoach;

namespace LOFit;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute("Login", typeof(LoginPage));
        Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
        Routing.RegisterRoute(nameof(ChangeOldPasswordPage), typeof(ChangeOldPasswordPage));
		Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));

        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));

        Routing.RegisterRoute(nameof(PlansPage), typeof(PlansPage));
        Routing.RegisterRoute(nameof(PlanWorkoutPage), typeof(PlanWorkoutPage));
        Routing.RegisterRoute(nameof(PlanMealPage), typeof(PlanMealPage));
        Routing.RegisterRoute(nameof(PlanListPage), typeof(PlanListPage));

        Routing.RegisterRoute(nameof(CoachsPage), typeof(CoachsPage));
		Routing.RegisterRoute(nameof(CoachPage), typeof(CoachPage));
		Routing.RegisterRoute(nameof(CertificatePage), typeof(CertificatePage));
		Routing.RegisterRoute(nameof(ConnectionsPage), typeof(ConnectionsPage));

		Routing.RegisterRoute(nameof(MeasurementPage), typeof(MeasurementPage));
		Routing.RegisterRoute(nameof(MealsPage), typeof(MealsPage));
		Routing.RegisterRoute(nameof(MealPage), typeof(MealPage));
		Routing.RegisterRoute(nameof(ProductsPage), typeof(ProductsPage));

		Routing.RegisterRoute(nameof(WorkoutsPage), typeof(WorkoutsPage));
		Routing.RegisterRoute(nameof(WorkoutPage), typeof(WorkoutPage));
		Routing.RegisterRoute(nameof(WorkoutsListPage), typeof(WorkoutsListPage));

		//Admin
        Routing.RegisterRoute(nameof(CentralAdminPage), typeof(CentralAdminPage));
        Routing.RegisterRoute(nameof(VerifyCoachPage), typeof(VerifyCoachPage));
        Routing.RegisterRoute(nameof(VerifyCertificatePage), typeof(VerifyCertificatePage));
        Routing.RegisterRoute(nameof(VerifyOpinionPage), typeof(VerifyOpinionPage));
        Routing.RegisterRoute(nameof(VerifyReportPage), typeof(VerifyReportPage));
        Routing.RegisterRoute(nameof(VerifyAppUsersPage), typeof(VerifyAppUsersPage));
    }
}
