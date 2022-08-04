using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Helpers;

namespace TSSedaplanifica.Controllers
{
    [Authorize(Roles = $"{nameof(TypeUser.Administrador)}")]

    public class MeasureUnitsController : Controller
    {
        private readonly IMeasureUnitHelper _measureUnitHelper;

        public MeasureUnitsController(IMeasureUnitHelper measureUnitHelper)
        {
            _measureUnitHelper = measureUnitHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _measureUnitHelper.ListAsync());
        }

        // GET: MeasureUnits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MeasureUnit measureUnit = await _measureUnitHelper.ByIdAsync((int)id);

            if (measureUnit == null)
            {
                return NotFound();
            }

            return View(measureUnit);
        }

        // GET: MeasureUnits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MeasureUnits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NameShort")] MeasureUnit measureUnit)
        {
            if (ModelState.IsValid)
            {
                Response response = await _measureUnitHelper.AddUpdateAsync(measureUnit);


                if (response.IsSuccess)
                {
                    TempData["AlertMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            return View(measureUnit);
        }

        // GET: MeasureUnits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MeasureUnit measureUnit = await _measureUnitHelper.ByIdAsync((int)id);

            if (measureUnit == null)
            {
                return NotFound();
            }

            return View(measureUnit);
        }

        // POST: MeasureUnits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NameShort")] MeasureUnit measureUnit)
        {
            if (id != measureUnit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Response response = await _measureUnitHelper.AddUpdateAsync(measureUnit);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }
            return View(measureUnit);
        }

        // GET: MeasureUnits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MeasureUnit model = await _measureUnitHelper.ByIdAsync((int)id);

            return View(model);
        }

        // POST: MeasureUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Response response = await _measureUnitHelper.DeleteAsync((int)id);


            if (response.IsSuccess)
            {
                TempData["AlertMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            MeasureUnit model = await _measureUnitHelper.ByIdAsync((int)id);

            ModelState.AddModelError(string.Empty, response.Message);


            return View(model);
        }
    }
}
