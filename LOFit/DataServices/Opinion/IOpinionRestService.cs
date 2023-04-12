using LOFit.Models;

namespace LOFit.DataServices.Certificate
{
    public interface IOpinionRestService
    { 
        Task<int> Add(OpinionModel form);
        Task<string> Update(OpinionModel form);
        Task<string> Delete(int id);
        Task<OpinionModel> GetMyOpinion(int coachId);
        Task<OpinionModel> GetOne(int id);
        Task<List<OpinionModel>> GetCoachList(int id);
        Task<List<OpinionModel>> GetAppList();
    }
}