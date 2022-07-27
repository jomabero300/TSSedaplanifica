using Microsoft.AspNetCore.Mvc.Rendering;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface IMeasureUnitHelper
    {
        Task<Response> AddUpdateAsync(MeasureUnit model);
        Task<MeasureUnit> ByIdAsync(int id);
        Task<List<MeasureUnit>> ComboAsync();
        Task<Response> DeleteAsync(int id);
        Task<List<MeasureUnit>> ListAsync();

    }
}
