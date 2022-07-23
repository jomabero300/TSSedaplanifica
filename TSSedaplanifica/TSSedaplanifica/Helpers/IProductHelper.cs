using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface IProductHelper
    {
        Task<Response> AddUpdateAsync(Product model);
        Task<Product> ByIdAsync(int id);
        Task<List<Product>> ComboAsync();
        Task<Response> DeleteAsync(int id);
        Task<List<Product>> ListAsync();
    }
}
