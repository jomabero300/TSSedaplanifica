using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface ICategoryTypeHelper
    {
        Task<Response> AddUpdateAsync(CategoryType model);
        Task<CategoryType> ByIdAsync(int id);
        Task<List<CategoryType>> ComboAsync();
        Task<Response> DeleteAsync(int id);
        Task<List<CategoryType>> ListAsync();
    }
}
