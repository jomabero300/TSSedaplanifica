using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class CategoryTypeHelper : ICategoryTypeHelper
    {
        private readonly ApplicationDbContext _context;

        private const string _name = "Clase de categoría";

        public CategoryTypeHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(CategoryType model)
        {
            Response response = new Response() { IsSuccess = true };

            if (model.Id == 0)
            {
                _context.CategoryTypes.Add(model);

                response.Message = $"{_name} guardado satisfactoriamente.!!!";
            }
            else
            {
                _context.CategoryTypes.Update(model);

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

        public async Task<CategoryType> ByIdAsync(int id)
        {
            CategoryType model = await _context.CategoryTypes.FindAsync(id);

            return model;
        }

        public async Task<List<CategoryType>> ComboAsync()
        {
            List<CategoryType> model = await _context.CategoryTypes.ToListAsync();

            model.Add(new CategoryType { Id = 0, Name = "[Seleccione una clase de categoría..]" });

            return model.OrderBy(m => m.Name).ToList();
        }
        public async Task<List<CategoryType>> ComboAsync(int categoryid)
        {
            //List<CategoryType> model = await _context.CategoryTypeDers
            //                                            .Include(c=>c.CategoryType)
            //                                            .Where(c=>c.Category.Id== categoryid)
            //                                            .Select(s=>s.CategoryType)
            //                                            .ToListAsync();

            List<CategoryTypeDer> ctd = await _context.CategoryTypeDers
                                                        .Include(c => c.Category)
                                                        .Include(s=>s.CategoryType)
                                                        .Where(c => c.Category.Id == categoryid)
                                                        .ToListAsync();

            List<CategoryType> ct = new List<CategoryType>();

            foreach (var c in ctd)
            {
                ct.Add(new CategoryType { Id = c.CategoryType.Id, Name = c.CategoryType.Name });
            }

            List<CategoryType> model = await _context.CategoryTypes.Where(c => !ct.Contains(c)).ToListAsync();

            model.Add(new CategoryType { Id = 0, Name = "[Seleccione una clase de categoría..]" });

            return model.OrderBy(m => m.Name).ToList();
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true };

            CategoryType model = await _context.CategoryTypes.FindAsync(id);

            try
            {
                response.Message = $"{_name} borrado(a) satisfactoriamente!!!";

                _context.CategoryTypes.Remove(model);
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

        public async Task<List<CategoryType>> ListAsync()
        {
            List<CategoryType> model = await _context.CategoryTypes.ToListAsync();

            return model.OrderBy(m => m.Name).ToList();
        }
    }
}
