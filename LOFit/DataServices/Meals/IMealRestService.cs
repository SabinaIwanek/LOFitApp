using LOFit.Models.Menu;

namespace LOFit.DataServices.Meals
{
    public interface IMealRestService
    {
        Task<string> Add(MealModel form);
        Task<string> Update(MealModel form);
        Task<string> Delete(int id);
        Task<MealModel> GetOne(int id);
        Task<List<MealModel>> GetUserList(DateTime date, int userId);
    }
}