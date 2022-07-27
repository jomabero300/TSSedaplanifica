using Microsoft.AspNetCore.Identity;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public interface IUserHelper
    {
        Task<ApplicationUser> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(ApplicationUser user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(ApplicationUser user, string roleName);

        Task<bool> IsUserInRoleAsync(ApplicationUser user, string roleName);

    }
}
