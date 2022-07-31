using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class SolicitDetailHelper : ISolicitDetailHelper
    {
        private const string _name = "Elemento";

        private readonly ApplicationDbContext _context;

        public SolicitDetailHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(SolicitDetail model)
        {
            Response response = new Response() { IsSuccess = true };

            if (model.Id == 0)
            {
                _context.SolicitDetails.Add(model);

                response.Message = $"{_name} guardado satisfactoriamente.!!!";
            }
            else
            {
                _context.SolicitDetails.Update(model);

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

        public async Task<SolicitDetail> ByIdAsync(int id)
        {
            return await _context.SolicitDetails
                                    .Include(d => d.Solicit)
                                    .Where(x => x.Id == id)
                                .FirstOrDefaultAsync();
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true };

            SolicitDetail model = await _context.SolicitDetails.FindAsync(id);

            try
            {
                response.Message = $"{_name} borrado(a) satisfactoriamente!!!";

                _context.SolicitDetails.Remove(model);

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

        public async Task<List<SolicitDetail>> ListAsync(int id)
        {
            return await _context.SolicitDetails.Where(s=>s.Solicit.Id==id).ToListAsync();
        }
    }
}
