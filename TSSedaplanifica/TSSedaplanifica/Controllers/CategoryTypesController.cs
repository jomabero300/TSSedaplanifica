using iTextSharp.text;
using iTextSharp.text.pdf;
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

    public class CategoryTypesController : Controller
    {
        private readonly ICategoryTypeHelper _categoryType;
        private readonly IPdfDocumentHelper _pdfDocument;


        public CategoryTypesController(ICategoryTypeHelper categoryType, IPdfDocumentHelper pdfDocument)
        {
            _categoryType = categoryType;
            _pdfDocument = pdfDocument;
        }

        // GET: CategoryTypes
        public async Task<IActionResult> Index()
        {
              return View(await _categoryType.ListAsync());
        }

        // GET: CategoryTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CategoryType categoryType = await _categoryType.ByIdAsync((int)id);

            if (categoryType == null)
            {
                return NotFound();
            }

            return View(categoryType);
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
        public async Task<IActionResult> Create([Bind("Id,Name")] CategoryType categoryType)
        {
            if (ModelState.IsValid)
            {
                Response response =await _categoryType.AddUpdateAsync(categoryType);


                if (response.IsSuccess)
                {                    
                    TempData["AlertMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));    
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            return View(categoryType);
        }

        // GET: CategoryTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CategoryType categoryType = await _categoryType.ByIdAsync((int)id);

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CategoryType categoryType)
        {
            if (id != categoryType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Response response = await _categoryType.AddUpdateAsync(categoryType);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }
            return View(categoryType);
        }

        // GET: CategoryTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CategoryType model=await _categoryType.ByIdAsync((int)id);

            return View(model);
        }

        // POST: CategoryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Response response = await _categoryType.DeleteAsync((int)id);


            if (response.IsSuccess)
            {
                TempData["AlertMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            CategoryType model = await _categoryType.ByIdAsync((int)id);

            ModelState.AddModelError(string.Empty, response.Message);


            return View(model);
        }

        public async Task<IActionResult> ReportList()
        {
            MemoryStream ms = await _pdfDocument.ReportListAsync("Clases de categorías");

            return File(ms.ToArray(), "application/pdf");
        }
    }
}
