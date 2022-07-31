using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface ISolicitDetailHelper
    {
        /// <summary>
        /// Guarda o actualiza un detalle de de la solicitud
        /// </summary>
        /// <param name="model"></param>
        /// <returns>-Devuelve si lo  guardo o no</returns>
        Task<Response> AddUpdateAsync(SolicitDetail model);
        /// <summary>
        /// Busca un detalle de de la solicitud
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna un detalle si no encontro</returns>
        Task<SolicitDetail> ByIdAsync(int id);

        Task<SolicitDetail> ByIdAsync(int id,int producId);
        /// <summary>
        /// Borra un detalle de de la solicitud
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response> DeleteAsync(int id);
        /// <summary>
        /// Busca todos los detalle de de la solicitud
        /// </summary>
        /// <returns></returns>
        Task<List<SolicitDetail>> ListAsync(int id);
    }
}
