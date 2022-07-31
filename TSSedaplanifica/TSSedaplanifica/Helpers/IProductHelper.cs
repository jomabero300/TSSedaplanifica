using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface IProductHelper
    {
        /// <summary>
        /// Guarda o actualiza un elemento
        /// </summary>
        /// <param name="model"></param>
        /// <returns>-Devuelve si lo  guardo o no</returns>
        Task<Response> AddUpdateAsync(Product model);
        /// <summary>
        /// Busca un elemento en la collecion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna el elemento si no encontro</returns>
        Task<Product> ByIdAsync(int id);
        /// <summary>
        /// Se consulta todos los elementos existente
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> ComboAsync();
        /// <summary>
        /// Consulta los elementos por la clase de su tipo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Product>> ComboAsync(int id);
        /// <summary>
        /// orra un elemento especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response> DeleteAsync(int id);
        /// <summary>
        /// Busca todos los elemento para una lista
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> ListAsync();
    }
}
