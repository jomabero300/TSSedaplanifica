using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface IProductCategoryHelper
    {
        Task<Response> AddUpdateAsync(ProductCategory model);
        Task<ProductCategory> ByIdAsync(int id);
        //Task<Response> DeleteAsync(int ProductId, int CategoryId);
        Task<Response> DeleteAsync(int Id);
        Task<List<ProductCategory>> ListAsync(int id);
    }
}
