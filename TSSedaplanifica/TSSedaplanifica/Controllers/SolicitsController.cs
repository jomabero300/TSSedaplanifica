using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

    [Authorize(Roles = $"{nameof(TypeUser.Coordinador)},{nameof(TypeUser.Rector)}, {nameof(TypeUser.Secretario_municipal)}")]

    public class SolicitsController : Controller
    {
        private readonly ISolicitHelper _solicitHelper;
        private readonly IUserHelper _userHelper;
        private readonly ISchoolHelper _schoolHelper;
        private readonly ISolicitStateHelper _solicitStateHelper;

        private readonly ICategoryTypeHelper _categoryTypeHelper;
        private readonly ICategoryHelper _categoryHelper;
        private readonly IProductHelper _productHelper;

        private readonly ISolicitDetailHelper _solicitDetailHelper;

        public SolicitsController(
            ISolicitHelper solicitHelper,
            IUserHelper userHelper,
            ISchoolHelper schoolHelper,
            ISolicitStateHelper slicitStateHelper,
            ICategoryTypeHelper categoryTypeHelper,
            ICategoryHelper categoryHelper,
            IProductHelper productHelper,
            ISolicitDetailHelper solicitDetailHelper)
        {
            _solicitHelper = solicitHelper;
            _userHelper = userHelper;
            _schoolHelper = schoolHelper;
            _solicitStateHelper = slicitStateHelper;
            _categoryTypeHelper = categoryTypeHelper;
            _categoryHelper = categoryHelper;
            _productHelper = productHelper;
            _solicitDetailHelper = solicitDetailHelper;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser lnaem = await _userHelper.GetUserAsync(User.Identity.Name);

            List<SolicitViewModel> model = await _solicitHelper.ListAsync(lnaem.Id);
            
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Solicit solicit = await _solicitHelper.ByIdDetailAsync((int)id);

            if (solicit == null)
            {
                return NotFound();
            }

            return View(solicit);
        }

        public async Task<IActionResult> Create()
        {
            string lnaem = await _userHelper.ByNameUneRoleAsync(User.Identity.Name);

            ApplicationUser user = await _userHelper.GetUserAsync(User.Identity.Name);
            List<SolicitState> state = await _solicitStateHelper.SolicitudStateAsync(lnaem, true);

            SolicitAddAndEditViewModel model = new SolicitAddAndEditViewModel()
            {
                UserApprovedDeniedId = user.Id,
                UserClosedId= user.Id,
                UserReceivedId= user.Id,
                SolicitStatesId=state.Where(s=>s.Name== TypeSolicitState.Borrador.ToString()).FirstOrDefault().Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitAddAndEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userHelper.GetUserAsync(User.Identity.Name);

                SchoolUser su =await _schoolHelper.ByUserSchoolAsync(User.Identity.Name);

                School school = await _schoolHelper.ByIdAsync(su.School.Id);

                Solicit solicit = new Solicit()
                {
                    School= school,
                    DateOfSolicit=DateTime.Now,
                    Description=model.Description,
                    SolicitStates=await _solicitStateHelper.ByIdAsync(model.SolicitStatesId),
                    DateOfReceived=DateTime.Now,
                    UserReceived= user,
                    DateOfApprovedDenied=DateTime.Now,
                    UserApprovedDenied= user,
                    DateOfClosed=DateTime.Now,
                    UserClosed= user,
                };

                Response response = await _solicitHelper.AddUpdateAsync(solicit);

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

            Solicit solicit = await _solicitHelper.ByIdAsync((int)id);

            if (solicit == null)
            {
                return NotFound();
            }

            return View(solicit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateOfSolicit,Description,DateOfReceived,DateOfApprovedDenied,DateOfClosed")] Solicit model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (model.Id != 0 && model.Description.Trim().Length>0)
            {
                Solicit solicit = await _solicitHelper.ByIdAsync(model.Id);

                solicit.Description = model.Description;

                Response response =await _solicitHelper.AddUpdateAsync(solicit);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Solicit solicit = await _solicitHelper.ByIdAsync((int)id);

            if (solicit == null)
            {
                return NotFound();
            }

            return View(solicit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Response response = await _solicitHelper.DeleteAsync(id);

            TempData["AlertMessage"] = response.Message;

            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            Solicit solicit = await _solicitHelper.ByIdAsync((int)id);

            return View(solicit);
        }

        public async Task<IActionResult> ProductAdd(int id)
        {
            SolicitDetailProductViewModel model=new SolicitDetailProductViewModel()
                    {
                        Id = id,
                    };

            ViewData["CategoryTypeId"] = new SelectList(await _categoryTypeHelper.ComboAsync(), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(await _categoryHelper.ComboAsync(0), "Id", "Name");
            ViewData["ProductId"] = new SelectList(await _productHelper.ComboAsync(0), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductAdd(SolicitDetailProductViewModel model)
        {
            if(ModelState.IsValid)
            {

                SolicitDetail sd= await _solicitDetailHelper.ByIdAsync(model.Id, model.ProductId);

                if(sd != null)
                {
                    sd.Quantity += model.Quantity;
                }
                else
                {
                    Solicit solicit = await _solicitHelper.ByIdAsync(model.Id);

                    Product product = await _productHelper.ByIdAsync(model.ProductId);

                    ApplicationUser user = await _userHelper.GetUserAsync(User.Identity.Name);

                    sd=new SolicitDetail()
                    {
                        Solicit=solicit,
                        Product=product,
                        Quantity=model.Quantity,
                        DirectorQuantity = model.Quantity,
                        PlannerQuantity = model.Quantity,
                        DeliveredQuantity = model.Quantity,
                        DateOfClosed=DateTime.Now,
                        UserDelivered=user,
                        Description="s/d"
                    };
                }

                Response response = await _solicitDetailHelper.AddUpdateAsync(sd);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Details),new { Id= model.Id});
                }

            }

            ViewData["CategoryTypeId"] = new SelectList(await _categoryTypeHelper.ComboAsync(), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(await _categoryHelper.ComboAsync(model.CategoryTypeId), "Id", "Name",model.CategoryTypeId);
            ViewData["ProductId"] = new SelectList(await _productHelper.ComboAsync(model.CategoryId), "Id", "Name", model.CategoryId);

            return View(model);
        }
        
        public async Task<IActionResult> ProductDelete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            SolicitDetail model = await _solicitDetailHelper.ByIdAsync((int)id);

            if (id == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("ProductDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDeleteConfirmed(int id)
        {
            SolicitDetail model = await _solicitDetailHelper.ByIdAsync((int)id);

            Response response = await _solicitDetailHelper.DeleteAsync((int)id);

            if(response.IsSuccess)
            {
                return RedirectToAction(nameof(Details), new { id = model.Solicit.Id });
            }

            return View(model);
        }

        public async Task<IActionResult> RequestSend(int id)
        {
            Solicit model=await _solicitHelper.ByIdAsync(id);

            if(model==null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("RequestSend")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestSendConfirmed(int id)
        {
            Response response = await _solicitHelper.RequestSendAsync(id, TypeSolicitState.Enviado.ToString());

            TempData["AlertMessage"] = response.Message;

            if(response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            Solicit model = await _solicitHelper.ByIdAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryListById(int categoryTypeId)
        {
            List<Category> lista = await _categoryHelper.ComboAsync(categoryTypeId);


            return Json(lista);
        }

        [HttpGet]
        public async Task<IActionResult> ProductListById(int categoryId)
        {
            List<Product> lista = await _productHelper.ComboAsync(categoryId);

            return Json(lista);
        }

        [HttpGet]
        public async Task<IActionResult> ToConsolidate(int id)
        {
            Solicit model = await _solicitHelper.ByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("ToConsolidate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToConsolidateConfirmed(int id)
        {
            Response response = await _solicitHelper.RequestSendAsync(id,TypeSolicitState.Consolidado.ToString());

            TempData["AlertMessage"] = response.Message;

            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            Solicit model = await _solicitHelper.ByIdAsync(id);

            return View(model);
        }

        [NoDirectAccess]
        public async Task<IActionResult> SolicitEarring(int id)
        {
            Solicit solicit = await _solicitHelper.ByIdAsync(id);

            solicit.SolicitStates=await _solicitStateHelper.ByIdAsync(TypeSolicitState.Pendiente.ToString());
            
            Response response = await _solicitHelper.AddUpdateAsync(solicit);

            TempData["AlertMessage"] = response.Message;

            return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Details), new { id = solicit.Id });

        }

        [HttpGet]
        public async Task<IActionResult> SolicitPassed(int id)
        {
            Solicit model = await _solicitHelper.ByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("SolicitPassed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitPassedConfirmed(int id)
        {
            Solicit solicit = await _solicitHelper.ByIdAsync(id);

            solicit.SolicitStates = await _solicitStateHelper.ByIdAsync(TypeSolicitState.Consolidado.ToString());

            Response response = await _solicitHelper.AddUpdateAsync(solicit);

            TempData["AlertMessage"] = response.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SolicitDenied(int id)
        {
            Solicit model = await _solicitHelper.ByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("SolicitDenied")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitDeniedConfirmed(int id)
        {
            Solicit solicit = await _solicitHelper.ByIdAsync(id);

            solicit.SolicitStates = await _solicitStateHelper.ByIdAsync(TypeSolicitState.Denegado.ToString());

            Response response = await _solicitHelper.AddUpdateAsync(solicit);

            TempData["AlertMessage"] = response.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RectorEditQuantity(int id)
        {
            SolicitDetail model = await _solicitDetailHelper.ByIdAsync(id);
            ProductEditrectorPlanner p = new ProductEditrectorPlanner()
            {
                Id = model.Id,
                Name=model.Product.Name,
                Description=model.Description,
                Quantity=model.DirectorQuantity,
                SolicitId=model.Solicit.Id
            };

            return View(p);
        }

        [HttpPost, ActionName("RectorEditQuantity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RectorEditQuantityConfirmed([Bind("Id,SolicitId,Name,Description,Quantity")] ProductEditrectorPlanner model)
        {
            if(ModelState.IsValid)
            {
                SolicitDetail sd = await _solicitDetailHelper.ByIdAsync(model.Id);

                sd.DirectorQuantity = model.Quantity;
                sd.Description= model.Description;

                Response response = await _solicitDetailHelper.AddUpdateAsync(sd);

                TempData["AlertMessage"] = response.Message;

                return RedirectToAction(nameof(Details),new { id= model.SolicitId});
            }

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> RectorConsolidate()
        {
            SolicitConsolidateViewModel model =await _solicitHelper.ListConsolidateAsync(User.Identity.Name);

            return View(model);

        }

        [HttpPost, ActionName("RectorConsolidate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RectorConsolidateConfirmed([Bind("School,Description,SolicitCons,Details")]SolicitConsolidateViewModel model)
        {
            if(ModelState.IsValid)
            {
                Response response = await _solicitHelper.AddUpdateAsync(User.Identity.Name,model.Description,User.Identity.Name);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
