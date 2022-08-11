using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;

namespace TSSedaplanifica.Helpers
{
    public class SolicitStateHelper : ISolicitStateHelper
    {
        private readonly ApplicationDbContext _context;

        private const string _name = "Estado de solicitud";

        public SolicitStateHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(SolicitState model)
        {
            Response response = new Response() { IsSuccess = true };

            if (model.Id == 0)
            {
                _context.SolicitStates.Add(model);

                response.Message = $"{_name} guardado(a) satisfactoriamente.!!!";
            }
            else
            {
                _context.SolicitStates.Update(model);

                response.Message = $"{_name} actualizado(a) satisfactoriamente.!!!";
            }


            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplica"))
                {
                    response.Message = $"Ya existe una {_name} con el mismo nombre.!!!";
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

        public async Task<SolicitState> ByIdAsync(int id)
        {
            SolicitState model = await _context.SolicitStates.FindAsync(id);

            return model;
        }

        public async Task<SolicitState> ByIdAsync(string name)
        {
            SolicitState model = await _context.SolicitStates.Where(s => s.Name == name).FirstOrDefaultAsync();

            return model;
        }

        public async Task<List<SolicitState>> ComboAsync()
        {
            List<SolicitState> model = await _context.SolicitStates.Where(s=>s.Name != "Inicial").ToListAsync();

            model.Add(new SolicitState { Id = 0, Name = "[Seleccione una Categoría..]" });

            return model.OrderBy(m => m.Name).ToList();
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true };

            SolicitState model = await _context.SolicitStates.FindAsync(id);

            try
            {
                response.Message = $"{_name} borrado(a) satisfactoriamente.!!!";

                _context.SolicitStates.Remove(model);
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

        public async Task<List<SolicitState>> ListAsync()
        {
            List<SolicitState> model = await _context.SolicitStates.Where(s => s.Name != "Inicial").ToListAsync();

            return model.OrderBy(m => m.Name).ToList();
        }

        public async Task<List<SolicitState>> SolicitudStateAsync(string stateId, bool lbEsta)
        {
            string[] ltname;

            if (stateId != "P" && lbEsta == true)
            {
                ltname = new string[] { TypeSolicitState.Borrador.ToString(), TypeSolicitState.Enviado.ToString() };
            }
            else if (stateId == "R")
            {
                ltname = new string[] { TypeSolicitState.Denegado.ToString(), TypeSolicitState.Pendiente.ToString() };
            }
            else
            {
                ltname = new string[] { TypeSolicitState.Aceptado.ToString(), TypeSolicitState.Proceso.ToString()};
            }

            List<SolicitState> solicitStates = await _context.SolicitStates.Where(s => ltname.Contains(s.Name)).ToListAsync();

            solicitStates.Add(new SolicitState { Id = 0, Name = "[Seleccione un estado..]" });

            return solicitStates.OrderBy(s => s.Name).ToList();
        }

        public async Task<SolicitState> ByNotInitialAsync(string name)
        {
            SolicitState model = await _context.SolicitStates.Where(s => s.Name == name && s.Name != "Inicial").FirstOrDefaultAsync();

            return model;
        }
    }
}