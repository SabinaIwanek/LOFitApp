using LOFit.Models;

namespace LOFit.DataServices.Certificate
{
    public interface ICertificateRestService
    { 
        Task<int> Add(CertificateModel form);
        Task<string> Update(CertificateModel form);
        Task<string> Delete(int id);
        Task<CertificateModel> GetOne(int id);
        Task<List<CertificateModel>> GetCoachList(int id);
        Task<List<CertificateModel>> GetAppList();
    }
}