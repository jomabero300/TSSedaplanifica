using TSSedaplanifica.Common;

namespace TSSedaplanifica.Helpers
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string servicePrefix, string controller);
    }
}
