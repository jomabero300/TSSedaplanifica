using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class MeasureUnitHelper : IMeasureUnitHelper
    {
        private readonly ApplicationDbContext _context;

        private const string _name = "Unidad de medida";

        public MeasureUnitHelper(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Response> AddUpdateAsync(MeasureUnit model)
        {
            Response response = new Response() { IsSuccess = true };

            if (model.Id == 0)
            {
                _context.MeasureUnits.Add(model);

                response.Message = $"{_name} guardado satisfactoriamente.!!!";
            }
            else
            {
                _context.MeasureUnits.Update(model);

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

        public async Task<MeasureUnit> ByIdAsync(int id)
        {
            MeasureUnit model = await _context.MeasureUnits.FindAsync(id);

            return model;
        }

        public async Task<List<MeasureUnit>> ComboAsync()
        {
            List<MeasureUnit> model = await _context.MeasureUnits.ToListAsync();

            model.Add(new MeasureUnit { Id = 0, Name = "[Seleccione una Categoría..]" });

            return model.OrderBy(m => m.Name).ToList();

            //List<SelectListItem> list = await _context.MeasureUnits.Select(x => new SelectListItem
            //{
            //    Text = x.Name,
            //    Value = $"{x.Id}"
            //})
            //     .OrderBy(x => x.Text)
            //     .ToListAsync();

            //list.Insert(0, new SelectListItem
            //{
            //    Text = "[Seleccione unidad de medida...]",
            //    Value = "0"
            //});

            //return list;
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true };

            MeasureUnit model = await _context.MeasureUnits.FindAsync(id);

            try
            {
                response.Message = $"{_name} borrado(a) satisfactoriamente!!!";

                _context.MeasureUnits.Remove(model);
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

        public async Task<List<MeasureUnit>> ListAsync()
        {
            List<MeasureUnit> model = await _context.MeasureUnits.ToListAsync();

            return model.OrderBy(m => m.Name).ToList();
        }
    }
}
