using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class CategoryTypeDerHelper : ICategoryTypeDerHelper
    {
        private const string _name = "Categoría en este clase de categoría";

        private readonly ApplicationDbContext _context;

        public CategoryTypeDerHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(CategoryTypeDer model)
        {
            Response response = new Response() { IsSuccess = true };

            try
            {

                _context.CategoryTypeDers.Add(model);

                response.Message = $"{_name} guardado satisfactoriamente.!!!";

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

        public async Task<CategoryTypeDer> ByIdAsync(int id)
        {
            CategoryTypeDer model = await _context.CategoryTypeDers.Include(c=>c.Category).Where(d=>d.Id== id).FirstOrDefaultAsync();

            return model;
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() 
            { 
                Message = $"Clases de categoría borrado(a) satisfactoriamente",
                
                IsSuccess = true 
            };

            CategoryTypeDer model = await _context.CategoryTypeDers.Where(pc => pc.Id == id).FirstOrDefaultAsync();

            try
            {
                response.Message = $"{_name} borrado(a) satisfactoriamente!!!";

                _context.CategoryTypeDers.Remove(model);

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

    }
}
