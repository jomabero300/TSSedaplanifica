using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Helpers.PDF;

namespace TSSedaplanifica.Helpers
{
    public class CategoryHelper : ICategoryHelper
    {
        private readonly ApplicationDbContext _context;
        private const string _name = "Categoría";
        private readonly IPdfDocumentHelper _pdfDoc;

        public CategoryHelper(ApplicationDbContext context, IPdfDocumentHelper pdfDoc)
        {
            _context = context;
            _pdfDoc = pdfDoc;
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
            Category model = await _context.Categories
                                            .Include(c=>c.CategoryTypeDers)
                                            .ThenInclude(d=>d.CategoryType)
                                            .Where(c=>c.Id==id).FirstOrDefaultAsync();

            return model;
        }

        public async Task<List<Category>> ComboAsync(int id)
        {
            List<CategoryTypeDer> ct = await _context.CategoryTypeDers
                                 .Include(c => c.Category)
                                 .Include(d => d.CategoryType)
                                 .Where(x => x.CategoryType.Id == id).ToListAsync();


            List<Category> model = ct.Select(c => new Category
            {
                Id = c.Category.Id,
                Name = c.Category.Name
            }).ToList();

            model.Add(new Category { Id = 0, Name = "[Seleccione una Categoría..]" });

            return model.OrderBy(m => m.Name).ToList();
        }

        public async Task<List<Category>> ComboAsync(int categoryTypeId, int productId)
        {

            List<Category> cp = (from c in _context.Categories
                                 join p in _context.ProductCategories on c.Id equals p.Category.Id
                                 where p.Product.Id == productId
                                 select c).ToList();

            List<Category> ct = await (from c in _context.Categories
                                       join ctd in _context.CategoryTypeDers on c.Id equals ctd.Category.Id
                                       where ctd.CategoryType.Id == categoryTypeId && !cp.Contains(c)
                                       select c).ToListAsync();


            ct.Add(new Category { Id = 0, Name = "[Seleccione una Categoría..]" });

            return ct.OrderBy(m => m.Name).ToList();

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

                if (ex.InnerException.Message.Contains("REFERENCE"))
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

        public async Task<MemoryStream> ReportAsync(string title)
        {

            MemoryStream ms =await _pdfDoc.ReportAsync(title);
            return ms;
        }

    }
}
