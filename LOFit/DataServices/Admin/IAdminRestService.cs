using LOFit.Models;

namespace LOFit.DataServices.Admin
{
    public interface IAdminRestService
    {
        Task<List<CoachModel>> GetWgTypeCoach(int type);
        Task<string> SetCoach(int id, int type);
        Task<List<CertificateModel>> GetWgTypeCert(int type);
        Task<string> SetCert(int id, int type);
        Task<List<OpinionModel>> GetWgTypeOpinion(int type);
        Task<string> SetOpinion(int id, int type);
        Task<List<ReportModel>> GetWgTypeReports(int type);
        Task<string> SetReport(int id, int type);
        Task<List<AdminModel>> GetAppUsersAdmins();
        Task<List<UserModel>> GetAppUsers();
        Task<List<CoachModel>> GetAppUsersCoachs();
        Task<string> BlockAccount(int id, int type);
        Task<string> UnblockAccount(int id, int type);
        Task<string> DeleteAccount(int id, int type);
    }
}