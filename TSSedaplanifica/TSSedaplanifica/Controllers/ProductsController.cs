using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Helpers;
using TSSedaplanifica.Models;

namespace TSSedaplanifica.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductHelper _prodcutHelper;

        private readonly IMeasureUnitHelper _measureUnitHelper;
        private readonly ICategoryHelper _categoryHelper;
        private readonly IProductCategoryHelper _productCategoryHelper;

        public ProductsController(IProductHelper prodcutHelper, IMeasureUnitHelper measureUnitHelper, ICategoryHelper categoryHelper, IProductCategoryHelper productCategoryHelper)
        {
            _prodcutHelper = prodcutHelper;
            _measureUnitHelper = measureUnitHelper;
            _categoryHelper = categoryHelper;
            _productCategoryHelper = productCategoryHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _prodcutHelper.ListAsync());
        }

        // GET: CategoryTypes/Details/5
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

        // GET: CategoryTypes/Create
        public async Task<IActionResult> Create()
        {
            ProductCreateViewModel vm = new ProductCreateViewModel()
            {
                MeasureUnit =await _measureUnitHelper.ComboAsync()
            };

            return View(vm);
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

        // GET: CategoryTypes/Edit/5
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

            return View(model);
        }

        // POST: CategoryTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Product model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Response response = await _prodcutHelper.AddUpdateAsync(model);

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

            Product model = await _prodcutHelper.ByIdAsync((int)id);

            return View(model);
        }

        // POST: CategoryTypes/Delete/5
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

            ViewData["CategoryId"] = new SelectList(await _categoryHelper.ListAsync(id), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductCategory(ProductAddCategoryViewModel model)
        {
            if(ModelState.IsValid)
            {
                ProductCategory pc = new ProductCategory()
                {
                    Category=await _categoryHelper.ByIdAsync(model.CategoryId),
                    Product=await _prodcutHelper.ByIdAsync(model.ProductId)
                };

                Response response = await _productCategoryHelper.AddUpdateAsync(pc);

                if (response.IsSuccess)
                {
                    TempData["AlertMessage"] = response.Message;

                    return RedirectToAction(nameof(Details), new { id= model.ProductId });
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            return View(model);
        }

        public async Task<IActionResult> DeleteProductCategory(int ProductId, int CategoryId)
        {
            Response response = await _productCategoryHelper.DeleteAsync(1, 1);

            return RedirectToAction(nameof(Details), new { id = ProductId });
        }
    }
}
