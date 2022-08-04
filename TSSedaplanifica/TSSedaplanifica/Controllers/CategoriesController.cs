using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Helpers;
using TSSedaplanifica.Models;
using static TSSedaplanifica.Helpers.ModalHelper;

namespace TSSedaplanifica.Controllers
{
    [Authorize(Roles = $"{nameof(TypeUser.Administrador)}")]

    public class CategoriesController : Controller
    {
        private readonly ICategoryHelper _category;
        private readonly ICategoryTypeHelper _categoryTypeHelper;
        private readonly ICategoryTypeDerHelper _categoryTypeDerHelper;

        public CategoriesController(ICategoryHelper category,
            ICategoryTypeHelper categoryTypeHelper,
            ICategoryTypeDerHelper categoryTypeDerHelper)
        {
            _category = category;
            _categoryTypeHelper = categoryTypeHelper;
            _categoryTypeDerHelper = categoryTypeDerHelper;
        }

        // GET: CategoryTypes
        public async Task<IActionResult> Index()
        {
            return View(await _category.ListAsync());
        }

        // GET: CategoryTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category model = await _category.ByIdAsync((int)id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: CategoryTypes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryTypeId"] = new SelectList(await _categoryTypeHelper.ComboAsync(), "Id", "Name");

            return View();
        }

        // POST: CategoryTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryTypeId")] CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {

                Category category = new Category()
                {
                    Id=model.Id,
                    Name=model.Name
                };

                Response response = await _category.AddUpdateAsync(category);

                if (response.IsSuccess)
                {
                    CategoryType ct = await _categoryTypeHelper.ByIdAsync(model.CategoryTypeId);

                    CategoryTypeDer ctd = new CategoryTypeDer()
                    {
                        Category = category,
                        CategoryType=ct,
                    };
                    
                    response = await _categoryTypeDerHelper.AddUpdateAsync(ctd);

                    TempData["AlertMessage"] = response.Message;
                    
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            ViewData["CategoryTypeId"] = new SelectList(await _categoryTypeHelper.ComboAsync(), "Id", "Name",model.CategoryTypeId);

            return View(model);
        }

        // GET: CategoryTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category categoryType = await _category.ByIdAsync((int)id);

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category categoryType)
        {
            if (id != categoryType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Response response = await _category.AddUpdateAsync(categoryType);

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

            Category model = await _category.ByIdAsync((int)id);

            return View(model);
        }

        // POST: CategoryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            Response response = await _category.DeleteAsync((int)id);

            if (response.IsSuccess)
            {
                TempData["AlertMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            Category model = await _category.ByIdAsync((int)id);

            ModelState.AddModelError(string.Empty, response.Message);


            return View(model);
        }

        public async Task<IActionResult> CategoryTypeDerAdd(int categoryid)
        {
            CategoryViewModel c = new CategoryViewModel() 
            {
                Id = categoryid,
                Name="xx"
            };

            ViewData["CategoryTypeId"] = new SelectList(await _categoryTypeHelper.ComboAsync(categoryid), "Id", "Name");

            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryTypeDerAdd(CategoryViewModel model)
        {
            CategoryTypeDer ct = new CategoryTypeDer()
            {
                Category=await _category.ByIdAsync(model.Id),
                CategoryType=await _categoryTypeHelper.ByIdAsync(model.CategoryTypeId)
            };

            Response response = await _categoryTypeDerHelper.AddUpdateAsync(ct);
            
            TempData["AlertMessage"] = response.Message;
            
            if(response.IsSuccess)
            {

                return RedirectToAction(nameof(Details),new {id=model.Id});
            }

            ViewData["CategoryTypeId"] = new SelectList(await _categoryTypeHelper.ComboAsync(model.Id), "Id", "Name",model.CategoryTypeId);

            return View(model);
        }

        [NoDirectAccess]
        public async Task<IActionResult> CategoryTypeDerDelete(int id)
        {
            CategoryTypeDer ctd = await _categoryTypeDerHelper.ByIdAsync(id);

            Response response = await _categoryTypeDerHelper.DeleteAsync(ctd.Id);

            TempData["AlertMessage"] = response.Message;
            
            return RedirectToAction(nameof(Details), new { id = ctd.Category.Id });
        }
    }
}
