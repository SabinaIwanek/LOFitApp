using LOFit.Models.Menu;
using LOFit.Models.MenuCoach;

namespace LOFit.DataServices.Plan
{
    public interface IPlanRestService
    {
        Task<List<List<WorkoutDayModel>>> GetWorkouts(int id);
        Task<List<List<MealModel>>> GetMeals(int id);
        Task<List<PlanModel>> GetByType(int type);
        Task<PlanModel> GetOne(int id);
        Task<int> Add(PlanModel form);
        Task<string> Update(PlanModel form);
        Task<string> Delete(int id);
    }
}