using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Models;

namespace TSSedaplanifica.Helpers
{
    public class SchoolUserHelper : ISchoolUserHelper
    {
        private const string _name = "Usuario institución";

        private readonly ApplicationDbContext _context;

        public SchoolUserHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(SchoolRectorCoordinator model)
        {
            Response response=new Response {IsSuccess=true,Message= $"{_name} guardado satisfactoriamente.!!!" };

            School school=await _context.Schools.FindAsync(model.SchoolId);

            SchoolUser schoolUser = await _context.SchoolUsers.Include(u=>u.ApplicationUser).Where(s=>s.School.Id==model.SchoolId && s.isEnable==true).FirstOrDefaultAsync();


            if (schoolUser != null)
            {
                response = await ByIdEnableAsync(schoolUser.ApplicationUser.Id);

            }

            SchoolUser schoolUserExiste = await _context.SchoolUsers.Where(s => s.ApplicationUser.Id == model.UserId && s.School.Id == model.SchoolId && s.ApplicationRole == model.rol).FirstOrDefaultAsync();
            if (schoolUserExiste != null)
            {
                schoolUserExiste.EndOfDate = DateTime.Now;
                schoolUserExiste.isEnable=false;
                _context.SchoolUsers.Update(schoolUserExiste);
            }

            ApplicationUser user = await _context.Users.FindAsync(model.UserId);

            SchoolUser su = new SchoolUser()
            {
                ApplicationRole = model.rol,
                ApplicationUser = user,
                isEnable = true,
                School = school,
                EndOfDate = model.EndOfDate,
                HireOfDate = model.HireOfDate,
            };

            _context.SchoolUsers.Add(su) ;

            response.Message = $"{_name} guardado satisfactoriamente.!!!";
            response.Result = su;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplica"))
                {
                    response.Message = $"Ya existe una {_name} con el mismo nombre.";
                }
                else
                {
                    response.Message = dbUpdateException.InnerException.Message;
                }

                response.IsSuccess = false;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;

                response.IsSuccess = false;
            }
            
            return response;
        }

        public async Task<SchoolUser> ByIdAsync(int id)
        {
            SchoolUser schoolUser = await _context.SchoolUsers.FindAsync(id);

            return schoolUser;
        }

        public async Task<SchoolUser> ByIdAsync(string id)
        {
            SchoolUser model = await _context.SchoolUsers
                                                .Include(s=>s.School)
                                                .ThenInclude(s=>s.City)
                                                .Include(s=>s.School.SchoolCampus)
                                             .Where(s => s.ApplicationUser.Id == id && s.isEnable==true)
                                             .FirstOrDefaultAsync();

            return model;
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true, Message = $"{_name} guardado satisfactoriamente.!!!" };

            SchoolUser model = await _context.SchoolUsers.Where(s=>s.School.Id==id).FirstOrDefaultAsync();

            School sc = await _context.Schools.Include(s=>s.SchoolCampus).Where(x=>x.Id==id).FirstOrDefaultAsync();

            response.Result = sc.SchoolCampus.Id;


            model.isEnable = false;

            try
            {
                _context.SchoolUsers.Update(model);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplica"))
                {
                    response.Message = $"Ya existe una {_name} con el mismo nombre.";
                }
                else
                {
                    response.Message = dbUpdateException.InnerException.Message;
                }

                response.IsSuccess = false;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;

                response.IsSuccess = false;
            }

            return response;
        }

        private async Task< Response> ByIdEnableAsync(string UserId)
        {
            Response response = new Response { IsSuccess = true };

            SchoolUser schoolUser = await _context.SchoolUsers.Where(s => s.ApplicationUser.Id == UserId && s.isEnable == true).FirstOrDefaultAsync();

            if (schoolUser != null)
            {

                schoolUser.isEnable = false;
                schoolUser.EndOfDate = DateTime.Now;

                _context.SchoolUsers.Update(schoolUser);

                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplica"))
                    {
                        response.Message = $"Ya existe una {_name} con el mismo nombre.";
                    }
                    else
                    {
                        response.Message = dbUpdateException.InnerException.Message;
                    }

                    response.IsSuccess = false;
                }
                catch (Exception exception)
                {
                    response.Message = exception.Message;

                    response.IsSuccess = false;
                }
            }

            return response;
        }
    }
}
