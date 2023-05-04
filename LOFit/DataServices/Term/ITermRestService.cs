using LOFit.Models.Menu;
using LOFit.Models.MenuCoach;

namespace LOFit.DataServices.Term
{
    public interface ITermRestService
    {
        Task<int> Add(TermModel form);
        Task<string> Update(TermModel form);
        Task<string> Delete(int id);
        Task<TermModel> GetOne(int id);
        Task<TermModel> GetNext(int id);
        Task<List<TermModel>> GetByDay(DateTime date);
        Task<List<TermModel>> GetAll();
    }
}