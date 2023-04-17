using LOFit.Models.Menu;

namespace LOFit.DataServices.Workouts
{
    public interface IWorkoutsRestService
    {
        Task<string> Add(WorkoutDayModel form);
        Task<string> Update(WorkoutDayModel form);
        Task<string> Delete(int id);
        Task<WorkoutDayModel> GetOne(int id);
        Task<List<WorkoutDayModel>> GetUserList(DateTime date, int idUsera);
    }
}