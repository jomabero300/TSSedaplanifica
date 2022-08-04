using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface ISolicitStateHelper
    {
        Task<Response> AddUpdateAsync(SolicitState model);
        Task<SolicitState> ByIdAsync(int id);
        Task<SolicitState> ByIdAsync(string name);
        Task<List<SolicitState>> ComboAsync();
        Task<Response> DeleteAsync(int id);
        Task<List<SolicitState>> ListAsync();

        Task<List<SolicitState>> SolicitudStateAsync(string stateId, bool lbEsta);
    }
}
