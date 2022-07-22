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

        public async Task<List<Zone>> ListAsync()
        {
            List<Zone> model = await _context.Zones.ToListAsync();

            return model.OrderBy(m => m.Name).ToList();
        }
    }
}
