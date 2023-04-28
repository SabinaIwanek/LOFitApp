using LOFit.DataServices.Admin;
using LOFit.DataServices.Certificate;
using LOFit.DataServices.Coach;
using LOFit.DataServices.Connection;
using LOFit.DataServices.Login;
using LOFit.DataServices.Meals;
using LOFit.DataServices.Measurement;
using LOFit.DataServices.Product;
using LOFit.DataServices.Registration;
using LOFit.DataServices.User;
using LOFit.DataServices.Workout;
using LOFit.DataServices.Workouts;
using LOFit.Pages.Admin;
using LOFit.Pages.Admin.VerifyLists;
using LOFit.Pages.Coachs;
using LOFit.Pages.Login;
using LOFit.Pages.Meals;
using LOFit.Pages.Measures;
using LOFit.Pages.Menu;
using LOFit.Pages.Workouts;

namespace LOFit;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddHttpClient<IAdminRestService, AdminRestService>();
        builder.Services.AddHttpClient<ICertificateRestService, CertificateRestService>();
        builder.Services.AddHttpClient<ICoachRestService, CoachRestService>();
        builder.Services.AddHttpClient<IConnectionRestService, ConnectionRestService>();
        builder.Services.AddHttpClient<ILoginRestService, LoginRestService>();
        builder.Services.AddHttpClient<IMeasurementRestService, MeasurementRestService>();
        builder.Services.AddHttpClient<IMealRestService, MealRestService>();
        builder.Services.AddHttpClient<IOpinionRestService, OpinionRestService>();
        builder.Services.AddHttpClient<IProductRestService, ProductRestService>();
        builder.Services.AddHttpClient<IRegistrationRestService, RegistrationRestService>();
        builder.Services.AddHttpClient<IUserRestService, UserRestService>();
        builder.Services.AddHttpClient<IWorkoutRestService, WorkoutRestService>();
        builder.Services.AddHttpClient<IWorkoutsRestService, WorkoutsRestService>();

        builder.Services.AddSingleton<LoginPage>();

        // Login
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ChangePasswordPage>();
        builder.Services.AddTransient<ChangeOldPasswordPage>();
        builder.Services.AddTransient<RegistrationPage>();

        // Menu
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<EditProfilePage>();

        // Coach
        builder.Services.AddTransient<CoachsPage>();
        builder.Services.AddTransient<CoachPage>();
        builder.Services.AddTransient<CertificatePage>();
        builder.Services.AddTransient<ConnectionsPage>();

        builder.Services.AddTransient<MeasurementPage>();

        builder.Services.AddTransient<MealsPage>();
        builder.Services.AddTransient<MealPage>();
        builder.Services.AddTransient<ProductsPage>();

        builder.Services.AddTransient<WorkoutsPage>();
        builder.Services.AddTransient<WorkoutPage>();
        builder.Services.AddTransient<WorkoutsListPage>();

        // Admmin
        builder.Services.AddTransient<CentralAdminPage>();
        builder.Services.AddTransient<VerifyCoachPage>();
        builder.Services.AddTransient<VerifyCertificatePage>();
        builder.Services.AddTransient<VerifyOpinionPage>();
        builder.Services.AddTransient<VerifyReportPage>();
        builder.Services.AddTransient<VerifyAppUsersPage>();


        return builder.Build();
    }
}

