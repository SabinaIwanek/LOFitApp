using LOFit.Models;

namespace LOFit.DataServices.Connection
{
    public interface IConnectionRestService
    { 
        Task<int> Add(ConnectionModel form);
        Task<string> Update(ConnectionModel form);
        Task<List<ConnectionModel>> GetCoachList(int id);
        Task<List<ConnectionModel>> GetUserList(int id);
    }
}