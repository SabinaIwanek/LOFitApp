using LOFit.Models.Menu;

namespace LOFit.DataServices.Product
{
    public interface IProductRestService 
    { 
        Task<int> Add(ProductModel form);
        Task<string> Update(ProductModel form);
        Task<string> Delete(int id);
        Task<ProductModel> GetOne(int id);
        Task<List<ProductModel>> GetUserList();
        Task<List<ProductModel>> GetAppList();
    }
}