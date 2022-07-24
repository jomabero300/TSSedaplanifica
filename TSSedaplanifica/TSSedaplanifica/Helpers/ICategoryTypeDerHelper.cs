using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface ICategoryTypeDerHelper
    {
        Task<Response> AddUpdateAsync(CategoryTypeDer model);
        Task<CategoryTypeDer> ByIdAsync(int id);
        Task<Response> DeleteAsync(int id);
    }
}
