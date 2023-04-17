using LOFit.Models.ProfileMenu;

namespace LOFit.DataServices.Certificate
{
    public interface ICertificateRestService
    { 
        Task<string> Add(CertificateModel form);
        Task<string> Update(CertificateModel form);
        Task<string> Delete(int id);
        Task<CertificateModel> GetOne(int id);
        Task<List<CertificateModel>> GetCoachList(int id);
        Task<List<CertificateModel>> GetAppList();
    }
}