using Microsoft.AspNetCore.Identity;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Models.ApplicationUser;

namespace TSSedaplanifica.Helpers
{
    public interface IUserHelper
    {
        Task AddUserToRoleAsync(ApplicationUser user, string roleName);

        Task<IdentityResult> AddUserAsync(ApplicationUser user, string password);

        Task<RoleUserModelView> ByIdRoleUserAsync(string id);

        Task CheckRoleAsync(string roleName);

        Task<ApplicationUser> GetUserAsync(string email);

        Task<bool> IsUserInRoleAsync(ApplicationUser user, string roleName);

        Task<List<RoleUserModelView>> ListRoleUserAsync();

        Task<List<RolesModelView>> ListRolesAsync();

        Task<List<ApplicationUser>> ListUserNotAssignedAsync(string roleName);

        Task<Response> UserRoleAddEditAsync(RoleUserModelView email);

        Task<Response> DeleteAsync(int id);

        Task<Response> DeleteUserRoleAsync(string id);

        Task<ApplicationUser> ByIdAsync(string id);

        Task<IdentityRole> ByIdRoleAsync(string id);

        Task<string> ByNameUneRoleAsync(string email);

        Task<IdentityUserRole<string>> ByIdUserRolAsync(string id);

        //Task<List<SolicitState>> SolicitudStateAsync(string stateId, bool lbEsta);

    }
}
