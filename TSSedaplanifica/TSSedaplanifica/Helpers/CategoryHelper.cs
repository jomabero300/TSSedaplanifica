using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class CategoryHelper : ICategoryHelper
    {
        private readonly ApplicationDbContext _context;

        private const string _name = "Categoría";

        public CategoryHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(Category model)
        {
            Response response = new Response() { IsSuccess = true };

            if (model.Id == 0)
            {
                _context.Categories.Add(model);

                response.Message = $"{_name} guardado(a) satisfactoriamente.!!!";
            }
            else
            {
                _context.Categories.Update(model);

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

        public async Task<Category> ByIdAsync(int id)
        {
            Category model = await _context.Categories.FindAsync(id);

            return model;
        }

        public async Task<List<Category>> ComboAsync()
        {
            List<Category> model = await _context.Categories.ToListAsync();

            model.Add(new Category { Id = 0, Name = "[Seleccione una Categoría..]" });

            return model.OrderBy(m => m.Name).ToList();
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true };

            Category model = await _context.Categories.FindAsync(id);

            try
            {
                response.Message = $"{_name} borrado(a) satisfactoriamente.!!!";

                _context.Categories.Remove(model);
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

        public async Task<List<Category>> ListAsync()
        {
            List<Category> model = await _context.Categories.ToListAsync();

            return model.OrderBy(m => m.Name).ToList();
        }

    }
}
