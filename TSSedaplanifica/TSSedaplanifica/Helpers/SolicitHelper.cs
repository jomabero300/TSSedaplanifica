using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;

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

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true, Message= $"{_name} borrado satisfactoriamente.!!!" };

            Solicit model =await _context.Solicits.Include(s=>s.SolicitDetails).Where(x=>x.Id==id).FirstOrDefaultAsync();

            try
            {

                _context.Solicits.Remove(model);

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

        public async Task<List<Solicit>> ListAsync(string id)
        {


            SchoolUser su = await _context.SchoolUsers.Include(s => s.ApplicationUser.Id == id && s.isEnable==true).FirstOrDefaultAsync();
            School sc = await _context.Schools.Where(s => s.Id == su.School.Id).FirstOrDefaultAsync();
            List<Solicit> solicits = await _context.Solicits
                                .Include(s => s.SolicitStates)
                                .Include(s => s.School).ThenInclude(x=>x.SchoolUsers)
                                .Where(s => s.School.SchoolUsers.FirstOrDefault().ApplicationUser.Id==id)
                                .ToListAsync();

            //List<Solicit> solicits =await (from s in _context.Solicits
            //                                 join e in _context.SolicitStates on s.SolicitStates.Id equals e.Id
            //                                 join c in _context.Schools on s.School.Id equals c.Id
            //                                 join su in _context.SchoolUsers on c.Id equals su.School.Id
            //                               where su.ApplicationUser.Id == id && su.isEnable == true
            //                               select s).ToListAsync();


            return solicits;
        }

        public async Task<Response> RequestSendAsync(int id)
        {
            Response response = new Response() { IsSuccess = false };

            int solicitudDetailCount = await _context.SolicitDetails.Where(s => s.Solicit.Id == id).CountAsync();

            if(solicitudDetailCount > 0)
            {
                Solicit solicit=await _context.Solicits.FindAsync(id);

                solicit.SolicitStates = await _context.SolicitStates.Where(s => s.Name == TypeSolicitState.Enviado.ToString()).FirstOrDefaultAsync();

                try
                {
                    _context.Update(solicit);

                    await _context.SaveChangesAsync();

                    response.Message = $"{_name} enviada exitosamente. !!!";
                    response.IsSuccess = true;
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
            else
            {
                response.Message = $"La {_name} NO tiene elementos a solicitar...";
            }

            return response;
        }
    }
}
