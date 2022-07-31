using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface ISolicitHelper
    {
        Task<Response> AddUpdateAsync(Solicit model);
        Task<Solicit> ByIdAsync(int id);
        Task<Solicit> ByIdDetailAsync(int id);
        Task<List<Solicit>> ComboAsync();
        Task<Response> DeleteAsync(int id);
        Task<Response> RequestSendAsync(int id);
        Task<List<Solicit>> ListAsync(string id);
    }
}
