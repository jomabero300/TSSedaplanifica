using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class ProductHelper : IProductHelper
    {
        private const string _name = "Producto";

        private readonly ApplicationDbContext _context;

        public ProductHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(Product model)
        {
            Response response = new Response() { IsSuccess = true };

            if (model.Id == 0)
            {
                _context.Products.Add(model);

                response.Message = $"{_name} guardado satisfactoriamente.!!!";
            }
            else
            {
                _context.Products.Update(model);

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

        public async Task<Product> ByIdAsync(int id)
        {
            Product model = await _context.Products
                                    .Include(m=>m.MeasureUnit)
                                    .Include(p=>p.ProductCategories)
                                    .ThenInclude(c=>c.Category)
                                    .Where(p=>p.Id==id)
                                    .FirstOrDefaultAsync();

            return model;
        }

        public async Task<List<Product>> ComboAsync()
        {
            List<Product> model = await _context.Products.ToListAsync();

            model.Add(new Product { Id = 0, Name = "[Seleccione un elemento..]" });

            return model.OrderBy(m => m.Name).ToList();
        }

        public async Task<List<Product>> ComboAsync(int id)
        {
            //List<Product> model = await _context.Products.Include(x=>x.ProductCategories).Where(p=>p.ProductCategories.FirstOrDefault().Category.Id==id).ToListAsync();
            List<Product> model = await (from p in _context.Products
                               join R in _context.ProductCategories on p.Id equals R.Product.Id
                               where R.Category.Id == id
                               select p).ToListAsync();

            model.Add(new Product { Id = 0, Name = "[Seleccione un elemento..]" });

            return model.OrderBy(m => m.Name).ToList();
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true };

            Product model = await _context.Products.FindAsync(id);

            try
            {
                response.Message = $"{_name} borrado(a) satisfactoriamente!!!";

                _context.Products.Remove(model);
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

        public async Task<List<Product>> ListAsync()
        {
            List<Product> model = await _context.Products
                                .Include(p=>p.MeasureUnit)
                                .Include(c => c.ProductCategories)
                                .ToListAsync();

            return model.OrderBy(m => m.Name).ToList();
        }

    }
}
