using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class SolicitHelper : ISolicitHelper
    {
        private const string _name = "Solicitud";

        private readonly ApplicationDbContext _context;

        public SolicitHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(Solicit model)
        {
            Response response = new Response() { IsSuccess = true };

            if (model.Id == 0)
            {
                _context.Solicits.Add(model);

                response.Message = $"{_name} guardado satisfactoriamente.!!!";
            }
            else
            {
                _context.Solicits.Update(model);

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

        public async Task<Solicit> ByIdAsync(int id)
        {
            return await _context.Solicits
                                .Where(s=>s.Id==id)
                                .FirstOrDefaultAsync();
        }

        public async Task<Solicit> ByIdDetailAsync(int id)
        {
            Solicit model = await _context.Solicits
                                .Include(s => s.SolicitStates)
                                .Include(s => s.SolicitDetails)
                                .ThenInclude(x=>x.Product)
                                .Where(s => s.Id == id)
                                .FirstOrDefaultAsync();
            return model;
        }

        public Task<List<Solicit>> ComboAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Solicit>> ListAsync(string id)
        {
            List<Solicit> solicits = await _context.Solicits
                                .Include(s => s.SolicitStates)
                                .Where(s => s.School.schoolUsers.FirstOrDefault().ApplicationUser.Id == id && s.School.schoolUsers.FirstOrDefault().isEnable==true)
                                .ToListAsync();

            return solicits;
        }
    }
}
