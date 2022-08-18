using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class ProductCategoryHelper : IProductCategoryHelper
    {
        private const string _name = "Producto";

        private readonly ApplicationDbContext _context;

        public ProductCategoryHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(ProductCategory model)
        {
            Response response = new Response() { IsSuccess = true };

            try
            {

                _context.ProductCategories.Add(model);

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

        public async Task<ProductCategory> ByIdAsync(int id)
        {
            ProductCategory model = await _context.ProductCategories.FindAsync(id);

            return model;
        }

        public async Task<Response> DeleteAsync(int Id)
        {
            Response response = new Response() { IsSuccess = true };

            //ProductCategory model = await _context.ProductCategories.Where(pc=>pc.Product.Id==ProductId && pc.Category.Id==CategoryId).FirstOrDefaultAsync();
            ProductCategory model = await _context.ProductCategories.FindAsync(Id);

            try
            {
                response.Message = $"{_name} borrado(a) satisfactoriamente!!!";

                _context.ProductCategories.Remove(model);

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

        public async Task<List<ProductCategory>> ListAsync(int id)
        {
            List<ProductCategory> model = await _context.ProductCategories
                                        .Include(p => p.Category)
                                        .Where(p=>p.Product.Id==id)
                                        .ToListAsync();

            return model.OrderBy(m => m.Category.Name).ToList();
        }

    }
}
