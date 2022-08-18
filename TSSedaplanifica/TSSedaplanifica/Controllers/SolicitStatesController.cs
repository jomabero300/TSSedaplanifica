using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Helpers;
using TSSedaplanifica.Helpers.PDF;

namespace TSSedaplanifica.Controllers
{
    [Authorize(Roles = $"{nameof(TypeUser.Administrador)}")]

    public class SolicitStatesController : Controller
    {
        private readonly ISolicitStateHelper _solicitStateHelper;
        private readonly IPdfDocumentHelper _pdfDocument;


        public SolicitStatesController(ISolicitStateHelper solicitStateHelper, IPdfDocumentHelper pdfDocument)
        {
            _solicitStateHelper = solicitStateHelper;
            _pdfDocument = pdfDocument;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _solicitStateHelper.ListAsync());
        }

        // GET: CategoryTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SolicitState model = await _solicitStateHelper.ByIdAsync((int)id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: CategoryTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SolicitState model)
        {
            if (ModelState.IsValid)
            {
                Response response = await _solicitStateHelper.AddUpdateAsync(model);


                if (response.IsSuccess)
                {
                    TempData["AlertMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            return View(model);
        }

        // GET: CategoryTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SolicitState categoryType = await _solicitStateHelper.ByIdAsync((int)id);

            if (categoryType == null)
            {
                return NotFound();
            }

            return View(categoryType);
        }

        // POST: CategoryTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SolicitState model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Response response = await _solicitStateHelper.AddUpdateAsync(model);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }
            return View(model);
        }

        // GET: CategoryTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SolicitState model = await _solicitStateHelper.ByIdAsync((int)id);

            return View(model);
        }

        // POST: CategoryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Response response = await _solicitStateHelper.DeleteAsync((int)id);


            if (response.IsSuccess)
            {
                TempData["AlertMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            SolicitState model = await _solicitStateHelper.ByIdAsync((int)id);

            ModelState.AddModelError(string.Empty, response.Message);


            return View(model);
        }
        public async Task<IActionResult> ReportList()
        {
            MemoryStream ms = await _pdfDocument.ReportAsync("Estados de solicitud");

            return File(ms.ToArray(), "application/pdf");
        }

    }
}
