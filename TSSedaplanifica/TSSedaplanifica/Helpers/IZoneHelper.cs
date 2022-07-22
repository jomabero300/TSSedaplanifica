using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface IZoneHelper
    {
        Task<List<Zone>> ListAsync();
    }
}
