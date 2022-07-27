using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface ICityHelper
    {
        Task<City> ByIdAsync(int id);

        Task<List<City>> ComboAsync(int stateId);
    }
}
