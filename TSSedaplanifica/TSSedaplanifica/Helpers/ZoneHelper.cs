using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class ZoneHelper : IZoneHelper
    {
        private readonly ApplicationDbContext _context;

        public ZoneHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Zone> ByIdAsync(int id)
        {
            return await _context.Zones.FindAsync(id);
        }

        public async Task<List<Zone>> ComboAsync()
        {
            List<Zone> model = await _context.Zones.ToListAsync();

            model.Add(new Zone { Id = 0, Name = "[Seleccione un zona..]" });

            return model.OrderBy(z=>z.Name).ToList();
        }

        public async Task<List<Zone>> ListAsync()
        {
            List<Zone> model = await _context.Zones.ToListAsync();

            return model.OrderBy(m => m.Name).ToList();
        }
    }
}
