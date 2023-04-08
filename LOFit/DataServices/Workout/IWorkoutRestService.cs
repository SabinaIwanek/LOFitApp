using LOFit.Models;

namespace LOFit.DataServices.Workout
{
    public interface IWorkoutRestService 
    { 
        Task<int> Add(WorkoutModel form);
        Task<string> Update(WorkoutModel form);
        Task<WorkoutModel> GetOne(int id);
        Task<List<WorkoutModel>> GetUserList();
        Task<List<WorkoutModel>> GetAppList();
    }
}