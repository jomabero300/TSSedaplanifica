using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface ISchoolHelper
    {
        Task<Response> AddUpdateAsync(School model);
        Task<School> ByIdAsync(int id);
        Task<List<School>> ComboAsync();
        Task<Response> DeleteAsync(int id);
        Task<List<School>> ListAsync();
        Task<List<School>> ListAsync(int id);
    }
}
