using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Helpers;
using TSSedaplanifica.Helpers.PDF;
using TSSedaplanifica.Models;

namespace TSSedaplanifica.Controllers
{
    [Authorize(Roles = $"{nameof(TypeUser.Administrador)}")]

    public class ProductsController : Controller
    {
        private readonly IProductHelper _prodcutHelper;

        private readonly IMeasureUnitHelper _measureUnitHelper;
        private readonly ICategoryHelper _categoryHelper;
        private readonly ICategoryTypeHelper _categoryTypeHelper;
        private readonly IProductCategoryHelper _productCategoryHelper;
        private readonly IPdfDocumentHelper _pdfDocument;

        public ProductsController(IProductHelper prodcutHelper,
            IMeasureUnitHelper measureUnitHelper,
            ICategoryHelper categoryHelper,
            IProductCategoryHelper productCategoryHelper,
            ICategoryTypeHelper categoryTypeHelper,
            IPdfDocumentHelper pdfDocument)
        {
            _prodcutHelper = prodcutHelper;
            _measureUnitHelper = measureUnitHelper;
            _categoryHelper = categoryHelper;
            _productCategoryHelper = productCategoryHelper;
            _categoryTypeHelper = categoryTypeHelper;
            _pdfDocument = pdfDocument;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _prodcutHelper.ListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product model = await _prodcutHelper.ByIdAsync((int)id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["MeasureUnitId"] = new SelectList(await _measureUnitHelper.ComboAsync(), "Id", "Name");

            return View();
        }

        // POST: CategoryTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,MeasureUnitId")] ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product=new Product()
                {
                    Name = model.Name,
                    Description=model.Description,
                    MeasureUnit= await _measureUnitHelper.ByIdAsync(model.MeasureUnitId)
                };

                Response response = await _prodcutHelper.AddUpdateAsync(product);

                if (response.IsSuccess)
                {
                    TempData["AlertMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product model = await _prodcutHelper.ByIdAsync((int)id);

            if (model == null)
            {
                return NotFound();
            }

            ProductCreateViewModel mv = new()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                MeasureUnitId= model.MeasureUnit.Id
            };

            ViewData["MeasureUnitId"] = new SelectList(await _measureUnitHelper.ComboAsync(), "Id", "Name", model.MeasureUnit.Id);

            return View(mv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,MeasureUnitId")] ProductCreateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    MeasureUnit = await _measureUnitHelper.ByIdAsync(model.MeasureUnitId)
                };
                Response response = await _prodcutHelper.AddUpdateAsync(product);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ViewData["MeasureUnitId"] = new SelectList(await _measureUnitHelper.ComboAsync(), "Id", "Name", model.MeasureUnitId);

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product model = await _prodcutHelper.ByIdAsync((int)id);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Response response = await _prodcutHelper.DeleteAsync((int)id);


            if (response.IsSuccess)
            {
                TempData["AlertMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            Product model = await _prodcutHelper.ByIdAsync((int)id);

            ModelState.AddModelError(string.Empty, response.Message);


            return View(model);
        }

        public async Task<IActionResult> AddProductCategory(int id)
        {
            ProductAddCategoryViewModel model = new ProductAddCategoryViewModel()
            {
                ProductId = id,
            };

            ViewData["CategoryTypeId"] = new SelectList(await _categoryTypeHelper.ComboAsync(), "Id", "Name");

            ViewData["CategoryId"] = new SelectList(await _categoryHelper.ComboAsync(0), "Id", "Name");

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductCategory(ProductAddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                ProductCategory pc = new ProductCategory()
                {
                    Category = await _categoryHelper.ByIdAsync(model.CategoryId),
                    Product = await _prodcutHelper.ByIdAsync(model.ProductId)
                };

                Response response = await _productCategoryHelper.AddUpdateAsync(pc);

                if (response.IsSuccess)
                {
                    TempData["AlertMessage"] = response.Message;

                    return RedirectToAction(nameof(Details), new { id = model.ProductId });
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            ViewData["CategoryTypeId"] = new SelectList(await _categoryTypeHelper.ComboAsync(), "Id", "Name", model.CategoryTypeId);

            ViewData["CategoryId"] = new SelectList(await _categoryHelper.ComboAsync(model.CategoryTypeId), "Id", "Name", model.CategoryId);

            return View(model);

        }

        public async Task<IActionResult> DeleteProductCategory(int ProductId, int CategoryId)
        {
            Response response = await _productCategoryHelper.DeleteAsync(CategoryId);

            return RedirectToAction(nameof(Details), new { id = ProductId });
        }

        [HttpGet]
        public async Task<IActionResult> GeneListById(int categoryTypeId)
        {
            List<Category> lista = await _categoryHelper.ComboAsync(categoryTypeId);


            return Json(lista);
        }

        public async Task<IActionResult> ReportList()
        {
            MemoryStream ms = await _pdfDocument.ReportListAsync("Elementos");

            return File(ms.ToArray(), "application/pdf");
        }

    }
}
