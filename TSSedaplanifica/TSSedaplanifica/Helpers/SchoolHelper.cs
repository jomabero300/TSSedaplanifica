using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;

namespace TSSedaplanifica.Helpers
{
    public class SchoolHelper : ISchoolHelper
    {
        private const string _name = "Institución educativa";

        private readonly ApplicationDbContext _context;

        public SchoolHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(School model)
        {
            Response response = new Response() { IsSuccess = true };

            if (model.Id == 0)
            {
                _context.Schools.Add(model);

                response.Message = $"{_name} guardado satisfactoriamente.!!!";
            }
            else
            {
                _context.Schools.Update(model);

                response.Message = $"{_name} actualizado satisfactoriamente.!!!";
            }


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

        public async Task<School> ByIdAsync(int id)
        {
            School model = await _context.Schools
                                    .Include(c => c.City)
                                    .Include(c => c.Zone)
                                    .Include(s=>s.SchoolCampus)
                                    .Where(s => s.Id == id)
                                    .FirstOrDefaultAsync();

            return model;
        }

        public async Task<SchoolUser> ByUserSchoolAsync(string email)
        {
            return await _context.SchoolUsers
                            .Include(s=>s.ApplicationUser)
                            .Include(x=>x.School)
                            .Where(s => s.ApplicationUser.Email == email && s.isEnable == true).FirstOrDefaultAsync();
        }

        public async Task<List<School>> ComboAsync(int id)
        {
            List<School> model = id==0 ? await _context.Schools.ToListAsync(): await _context.Schools.Where(s=>s.SchoolCampus.Id==id).ToListAsync();

            string lsName = id == 0 ? "institución" : "sede";

            model.Add(new School { Id = 0, Name = $"[Seleccione una {lsName}..]" });

            return model.OrderBy(m => m.Name).ToList();
        }

        public async Task<List<School>> ComboCityAsync(int id, bool sede = false)
        {
            List<School> model = sede==true? await _context.Schools.Where(s => s.City.Id == id && s.SchoolCampus==null).ToListAsync():
                                             await _context.Schools.Where(s => s.SchoolCampus.Id == id).ToListAsync();

            string lsName = sede == true ? "institución" : "sede";

            model.Add(new School { Id = 0, Name = $"[Seleccione una {lsName}..]" });

            return model.OrderBy(m => m.Name).ToList();

        }

        public async Task<List<School>> ComboStartStocktakingAsync()
        {
            List<School> modelSchols = await _context.Solicits.Include(s=>s.School)
                                                .Where(s => s.SolicitStates.Name == TypeSolicitState.Inicial.ToString())
                                                .Select(s=>s.School)
                                                .ToListAsync();
            List<School> model = await _context.Schools
                                                .Where(s => s.SchoolCampus==null && !modelSchols.Contains(s))
                                                .ToListAsync();

            model.Add(new School { Id = 0, Name = $"[Seleccione una institución..]" });

            return model.OrderBy(s=>s.Name).ToList();

        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true };

            School model = await _context.Schools.FindAsync(id);

            try
            {
                response.Message = $"{_name} borrado(a) satisfactoriamente!!!";

                _context.Schools.Remove(model);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                if (ex.Message.Contains("REFERENCE"))
                {
                    response.Message = $"{_name} no se puede eliminar porque tiene registros relacionados";
                }
                else
                {
                    response.Message = ex.Message;
                }

            }

            return response;
        }

        public async Task<List<School>> ListAsync()
        {
            List<School> model = await _context.Schools
                                                .Include(s => s.City)
                                                .Include(s => s.Zone)
                                                .Include(s =>s.SchoolUsers)
                                                .ThenInclude(s=>s.ApplicationUser)
                                                .Where(s => s.SchoolCampus == null)
                                                .ToListAsync();

            return model.OrderBy(m => m.Name).ToList();
        }

        public async Task<List<School>> ListAsync(int id)
        {
            List<School> model = await _context.Schools
                                                .Include(s => s.City)
                                                .Include(s => s.Zone)
                                                .Include(s=>s.SchoolUsers)
                                                .ThenInclude(s=>s.ApplicationUser)
                                                .Where(s => s.SchoolCampus.Id == id)
                                                .ToListAsync();

            return model.OrderBy(m => m.Name).ToList();
        }
    }
}
