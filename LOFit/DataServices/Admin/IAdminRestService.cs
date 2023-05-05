using LOFit.Models.Accounts;
using LOFit.Models.Menu;
using LOFit.Models.ProfileMenu;

namespace LOFit.DataServices.Admin
{
    public interface IAdminRestService
    {
        Task<AdminModel> GetOne(int id);

        //Zarzadzanie
        Task<List<CoachModel>> GetWgTypeCoach(int type);
        Task<string> SetCoach(int id, int type);
        Task<List<CertificateModel>> GetWgTypeCert(int type);
        Task<string> SetCert(int id, int type);
        Task<List<OpinionModel>> GetWgTypeOpinion(int type);
        Task<string> SetOpinion(int id, int type);
        Task<List<ProductModel>> GetWgTypeProducts(int type);
        Task<string> SetProduct(int id, int type);
        Task<List<AdminModel>> GetAppUsersAdmins();
        Task<List<UserModel>> GetAppUsers();
        Task<List<CoachModel>> GetAppUsersCoachs();
        Task<string> BlockAccount(int id, int type);
        Task<string> UnblockAccount(int id, int type);
        Task<string> DeleteAccount(int id, int type);
        Task<bool> IsBlock(int id, int type);
    }
}