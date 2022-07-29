using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Models;

namespace TSSedaplanifica.Helpers
{
    public interface ISchoolUserHelper
    {
        Task<Response> AddUpdateAsync(SchoolRectorCoordinator model);
        /// <summary>
        /// Consult por el id el mmodelo
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un modelo con la informacion</returns>
        Task<SchoolUser> ByIdAsync(int id);
        /// <summary>
        /// Consulta por ide del usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns>retorna la Id del usuario y la institución</returns>
        Task<SchoolUser> ByIdAsync(string id);

        //Task<Response> ByIdEnableAsync(string UserId);

        Task<Response> DeleteAsync(int id);
    }
}
