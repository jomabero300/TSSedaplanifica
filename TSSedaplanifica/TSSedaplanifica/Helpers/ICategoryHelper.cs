using Microsoft.AspNetCore.Mvc.Rendering;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface ICategoryHelper
    {
        Task<Response> AddUpdateAsync(Category model);
        Task<Category> ByIdAsync(int id);
        Task<List<Category>> ComboAsync(int id);
        Task<List<Category>> ComboAsync(int CategoryId, int prodcutId);
        Task<Response> DeleteAsync(int id);
        Task<List<Category>> ListAsync();
        //Task<List<Category>> ListAsync(int id);
    }
}
