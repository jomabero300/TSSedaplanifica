using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface ISchoolHelper
    {
        Task<Response> AddUpdateAsync(School model);

        Task<School> ByIdAsync(int id);

        Task<SchoolUser> ByUserSchoolAsync(string email);
        /// <summary>
        /// Listas las sedes de una institución
        /// </summary>
        /// <param name="id">codigo de la institucion para busqueda de sus sedes</param>
        /// <returns></returns>
        Task<List<School>> ComboAsync(int id);
        /// <summary>
        /// Lista las instituciones por municipio
        /// </summary>
        /// <param name="id">Municipio para busqueda de las instituciones</param>
        /// <returns></returns>
        Task<List<School>> ComboCityAsync(int id,bool sede=false);

        Task<List<School>> ComboStartStocktakingAsync();

        Task<Response> DeleteAsync(int id);
        /// <summary>
        /// Listado de Instituciones educativas
        /// </summary>
        /// <returns></returns>
        Task<List<School>> ListAsync();
        /// <summary>
        /// Busqueda de una isntitución educativa especifica
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Idenificación de la isntitución</returns>
        Task<List<School>> ListAsync(int id);
    }
}
