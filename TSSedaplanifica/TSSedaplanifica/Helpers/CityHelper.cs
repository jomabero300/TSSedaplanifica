using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class CityHelper : ICityHelper
    {
        private const string _name = "Producto";

        private readonly ApplicationDbContext _context;

        public CityHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<City> ByIdAsync(int id)
        {
            return await _context.Cities.FindAsync(id);
        }

        public async Task<List<City>> ComboAsync(int stateId)
        {
            List<City> model = await _context.Cities.Where(c => c.State.Id == stateId).ToListAsync();
            model.Add(new City { Id = 0, Name = "[Seleccione un municipio..]" });

            return model.OrderBy(c=>c.Name).ToList();

        }

    }
}
