using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Helpers;

namespace TSSedaplanifica.Controllers
{
    public class Solicits01Controller : Controller
    {
        private readonly ISolicitHelper _solicitHelper;


        private readonly IUserHelper _userHelper;

        private readonly ApplicationDbContext _context;

        public Solicits01Controller(ApplicationDbContext context, ISolicitHelper solicitHelper, IUserHelper userHelper)
        {
            _context = context;
            _solicitHelper = solicitHelper;
            _userHelper = userHelper;
        }

        // GET: Solicits

        // GET: Solicits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Solicits == null)
            {
                return NotFound();
            }

            var solicit = await _context.Solicits.FindAsync(id);
            if (solicit == null)
            {
                return NotFound();
            }
            return View(solicit);
        }

        // POST: Solicits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateOfSolicit,Description,DateOfReceived,DateOfApprovedDenied,DateOfClosed")] Solicit solicit)
        {
            if (id != solicit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitExists(solicit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(solicit);
        }

        // GET: Solicits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Solicits == null)
            {
                return NotFound();
            }

            var solicit = await _context.Solicits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solicit == null)
            {
                return NotFound();
            }

            return View(solicit);
        }

        // POST: Solicits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Solicits == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Solicits'  is null.");
            }
            var solicit = await _context.Solicits.FindAsync(id);
            if (solicit != null)
            {
                _context.Solicits.Remove(solicit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitExists(int id)
        {
          return _context.Solicits.Any(e => e.Id == id);
        }
    }
}
