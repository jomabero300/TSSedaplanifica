using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Models.ApplicationUser;

namespace TSSedaplanifica.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<ApplicationUser> ByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<RoleUserModelView> ByIdRoleUserAsync(string id)
        {
            RoleUserModelView user;

            var use = await (from U in _context.Users
                              join E in _context.UserRoles on U.Id equals E.UserId
                              join R in _context.Roles on E.RoleId equals R.Id
                              where U.Id == id
                              select new { U.Id, U.FullName,RoleId= R.Id }).FirstOrDefaultAsync();
            if(use == null)
            {
                var useNu = await (from U in _context.Users
                                  where U.Id == id
                                  select new { U.Id, U.FullName, RoleId = "" }).FirstOrDefaultAsync();

                user = new RoleUserModelView()
                {
                    UserId = useNu.Id,
                    FullName = useNu.FullName,
                    RoleId = useNu.RoleId
                };
            }
            else
            {
                user = new RoleUserModelView()
                {
                    UserId = use.Id,
                    FullName = use.FullName,
                    RoleId = use.RoleId
                };
            }

            return user;
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response { IsSuccess = true, Message = "Usiario borrado.." };

            try
            {
                var user = await _context.Users.FindAsync(id);

                _context.Users.Remove(user);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<Response> DeleteUserRoleAsync(string id)
        {
            Response response = new Response { IsSuccess = true, Message = "Usiario borrado.." };

            try
            {

                var userRole = await _context.UserRoles.Where(u=> u.UserId == id).FirstOrDefaultAsync();

                _context.UserRoles.Remove(userRole);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ApplicationUser> GetUserAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsUserInRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<List<RolesModelView>> ListRolesAsync()
        {
            var role = await _context.Roles.ToListAsync();

            List<RolesModelView> model = role.Select(r => new RolesModelView()
            {
                Id=r.Id,
                Name=r.Name
            }).ToList();

            model.Add(new RolesModelView() {Id="",Name= "[Seleccione un rol..]" });

            return model.OrderBy(m=>m.Name).ToList();

        }

        public async Task<List<RoleUserModelView>> ListRoleUserAsync()
        {

            var user = await (from U in _context.Users
                              join E in _context.UserRoles on U.Id equals E.UserId into userRroleRela
                              from UR in userRroleRela.DefaultIfEmpty()
                              select new { Id = U.Id, FullName = U.FullName, Name = UR.RoleId ?? string.Empty,U.Email }).ToListAsync();
            //join E in _context.UserRoles on U.Id equals E.UserId into userRroleRela
            //from UR in userRroleRela.DefaultIfEmpty()
            //select new { Id = U.Id, FullName = U.FullName, Name = UR.RoleId ?? string.Empty }).ToListAsync();

            //var response = await (from U in _context.Users
            //                   join E in _context.UserRoles on U.Id equals E.UserId
            //                   join R in _context.Roles on E.RoleId equals R.Id
            //                   select new { Id = U.Id, FullName = U.FullName, Name = R.Name ?? string.Empty }).ToListAsync();

            //var users = user.Select(u => new RoleUserModelView()
            //{
            //    UserId = u.Id,
            //    FullName = u.FullName,
            //    RoleId = u.Name
            //});
            //var userRole = await (from U in _context.Users
            //                  join E in _context.UserRoles on U.Id equals E.UserId into userRoles
            //                  from UR in userRoles.DefaultIfEmpty()
            //                  select new { Id = U.Id, FullName=U.FullName, Name= UR.UserId ?? string.Empty  }).ToListAsync();

            List<RoleUserModelView> users = user.Select(u => new RoleUserModelView()
            {
                UserId = u.Id,
                FullName = u.FullName,
                RoleId = u.Name==""?String.Empty: _context.Roles.Where(r=>r.Id== u.Name).FirstOrDefault().Name,
                email=u.Email
            }).ToList();

            return users.OrderBy(u => u.FullName).ToList();
        }

        public async Task<List<ApplicationUser>> ListUserNotAssignedAsync(string roleName)
        {
            var userInstitucion = await ((from U in _context.Users
                                         join S in _context.SchoolUsers on U.Id equals S.ApplicationUser.Id
                                         where S.isEnable == true
                                        select U).Distinct()).ToListAsync();

            var userNew = await (from U in _context.Users
                                 join E in _context.UserRoles on U.Id equals E.UserId
                                 join R in _context.Roles on E.RoleId equals R.Id
                                 where R.Name == roleName && !userInstitucion.Contains(U)
                                  select U).ToListAsync();


            List<ApplicationUser> model = userNew.Select(u => new ApplicationUser
            {
                Id = u.Id,
                Document=u.Document,
                UserName = u.UserName,
                Email = u.Email,
                FirstName=u.FirstName,
                LastName=u.LastName,
                ImageId=u.ImageId

            }).ToList();

            model.Add(new ApplicationUser { Id = "", FirstName = $"[Seleccione un {roleName}...]" });

            return model.OrderBy(u=>u.FullName).ToList();

        }

        public async Task<List<ApplicationUser>> ListUserNotAssignedAsync(int SchoolId)
        {

            List<ApplicationUser> userOn = await (from U in _context.Users
                                                        join S in _context.SchoolUsers on U.Id equals S.ApplicationUser.Id
                                                        join E in _context.UserRoles on U.Id equals E.UserId
                                                        join R in _context.Roles on E.RoleId equals R.Id
                                                  where S.isEnable == true && R.Name == TypeUser.Coordinador.ToString()
                                                        select U).ToListAsync();

            //List<ApplicationUser> userOff = await (from U in _context.Users
            //                                            join S in _context.SchoolUsers on U.Id equals S.ApplicationUser.Id
            //                                            join E in _context.UserRoles on U.Id equals E.UserId
            //                                            join R in _context.Roles on E.RoleId equals R.Id
            //                                       where S.isEnable == false && !userOn.Contains(U)
            //                                            select U).ToListAsync();


            List<ApplicationUser> userSchool = await (from U in _context.Users
                                                        join E in _context.UserRoles on U.Id equals E.UserId
                                                        join R in _context.Roles on E.RoleId equals R.Id
                                                        join S in _context.SchoolUsers on U.Id equals S.ApplicationUser.Id
                                                    where R.Name == TypeUser.Coordinador.ToString()
                                                    select U).ToListAsync();

            List<ApplicationUser> userNew = await (
                                                    (
                                                        from U in _context.Users
                                                        join S in _context.SchoolUsers on U.Id equals S.ApplicationUser.Id
                                                        where S.School.SchoolCampus.Id == SchoolId && S.isEnable == true
                                                        select U).Distinct()
                                                    .Union(
                                                        from U in _context.Users
                                                        join E in _context.UserRoles on U.Id equals E.UserId
                                                        join R in _context.Roles on E.RoleId equals R.Id
                                                        where R.Name == TypeUser.Coordinador.ToString() && !userSchool.Contains(U)
                                                        select U
                                                    ).Union(
                                                        from U in _context.Users
                                                        join S in _context.SchoolUsers on U.Id equals S.ApplicationUser.Id
                                                        join E in _context.UserRoles on U.Id equals E.UserId
                                                        join R in _context.Roles on E.RoleId equals R.Id
                                                        where S.isEnable == false && !userOn.Contains(U)
                                                        select U
                                                    )).ToListAsync();


            List<ApplicationUser> model = userNew.Select(u => new ApplicationUser
            {
                Id = u.Id,
                Document = u.Document,
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ImageId = u.ImageId

            }).ToList();

            model.Add(new ApplicationUser { Id = "", FirstName = $"[Seleccione un {TypeUser.Coordinador.ToString()}...]" });

            return model.OrderBy(u => u.FullName).ToList();
        }

        public async Task<Response> UserRoleAddEditAsync(RoleUserModelView model)
        {
            Response response=new Response { IsSuccess = false, Message="Usuario se encuentra activo en una institución" };

            SchoolUser userRole = await _context.SchoolUsers.Where(u => u.ApplicationUser.Id == model.UserId && u.isEnable==true).FirstOrDefaultAsync();
            if (userRole != null)
            {
                return response;
            }
            try
            {

                var user = await _userManager.FindByIdAsync(model.UserId);

                var role = _context.UserRoles.Where(u => u.UserId == model.UserId).FirstOrDefault();

                if(role!=null)
                {
                    var roleName = _context.Roles.Where(r => r.Id == role.RoleId).FirstOrDefault().Name;

                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }

                var roleNameAdd = _context.Roles.Where(r => r.Id == model.RoleId).FirstOrDefault().Name;

                await _userManager.AddToRoleAsync(user, roleNameAdd);

                response = new Response { IsSuccess = true, Message = "Usuario rol actualizado satisfactoriamente" };
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }        

            return response;
        }

        public async Task<IdentityUserRole<string>> ByIdUserRolAsync(string id)
        {
            return await _context.UserRoles.Where(u => u.UserId == id).FirstOrDefaultAsync();
        }

        public async Task<IdentityRole> ByIdRoleAsync(string id)
        {
            IdentityRole rol = await _context.Roles.Where(r => r.Name == id).FirstOrDefaultAsync();

            return rol;
        }

        public async Task<string> ByNameUneRoleAsync(string email)
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            IdentityUserRole<string> userRol = await  _context.UserRoles.Where(u => u.UserId == user.Id).FirstOrDefaultAsync();

            IdentityRole<string> rolname = await _context.Roles.Where(r => r.Id == userRol.RoleId).FirstOrDefaultAsync(); ;

            string lt = rolname.Name.Substring(0, 1);

            return lt;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }
    }
}
