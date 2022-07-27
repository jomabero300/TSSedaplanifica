using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface IZoneHelper
    {
        Task<Zone> ByIdAsync(int id);

        Task<List<Zone>> ComboAsync();

        Task<List<Zone>> ListAsync();
    }
}
