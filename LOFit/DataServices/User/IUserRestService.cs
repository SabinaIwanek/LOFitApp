using LOFit.Models.Accounts;

namespace LOFit.DataServices.User
{
    public interface IUserRestService
    { 
        Task<string> Update(UserModel form);
        Task<UserModel> GetOne(int id);
        Task<string> GetName(int id);
    }
}