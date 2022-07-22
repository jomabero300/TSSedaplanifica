using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface ICategoryHelper
    {
        Task<Response> AddUpdateAsync(Category model);
        Task<Category> ByIdAsync(int id);
        Task<List<Category>> ComboAsync();
        Task<Response> DeleteAsync(int id);
        Task<List<Category>> ListAsync();
    }
}
