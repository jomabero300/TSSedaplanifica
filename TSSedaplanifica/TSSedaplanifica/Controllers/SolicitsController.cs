using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Helpers;
using TSSedaplanifica.Helpers.PDF;
using TSSedaplanifica.Models;
using static TSSedaplanifica.Helpers.ModalHelper;

namespace TSSedaplanifica.Controllers
{
    [Authorize(Roles = $"{nameof(TypeUser.Coordinador)},{nameof(TypeUser.Rector)}, {nameof(TypeUser.Secretario_municipal)}, {nameof(TypeUser.Planificador)}, {nameof(TypeUser.Administrador)}")]

    public class SolicitsController : Controller
    {
        private readonly ISolicitHelper _solicitHelper;
        private readonly IUserHelper _userHelper;
        private readonly ISchoolHelper _schoolHelper;
        private readonly ISolicitStateHelper _solicitStateHelper;

        private readonly ICategoryTypeHelper _categoryTypeHelper;
        private readonly ICategoryHelper _categoryHelper;
        private readonly IProductHelper _productHelper;
        private readonly ICityHelper _cityHelper;


        private readonly ISolicitDetailHelper _solicitDetailHelper;        
        private readonly ISchoolUserHelper _schoolUserHelper;
        private readonly IPdfDocumentHelper _pdfDocument;

        public SolicitsController(
            ISolicitHelper solicitHelper,
            IUserHelper userHelper,
            ISchoolHelper schoolHelper,
            ISolicitStateHelper slicitStateHelper,
            ICategoryTypeHelper categoryTypeHelper,
            ICategoryHelper categoryHelper,
            IProductHelper productHelper,
            ISolicitDetailHelper solicitDetailHelper,
            ISchoolUserHelper schoolUserHelper,
            IPdfDocumentHelper pdfDocument,
            ICityHelper cityHelper)
        {
            _solicitHelper = solicitHelper;
            _userHelper = userHelper;
            _schoolHelper = schoolHelper;
            _solicitStateHelper = slicitStateHelper;
            _categoryTypeHelper = categoryTypeHelper;
            _categoryHelper = categoryHelper;
            _productHelper = productHelper;
            _solicitDetailHelper = solicitDetailHelper;
            _schoolUserHelper = schoolUserHelper;
            _pdfDocument = pdfDocument;
            _cityHelper = cityHelper;
        }

        public async Task<IActionResult> Index()
        {

            ApplicationUser lnaem = await _userHelper.GetUserAsync(User.Identity.Name);

            SchoolUser su = await _schoolUserHelper.ByIdAsync(lnaem.Id);

            if(su == null)
            {
                return RedirectToAction("NotAuthorized", "Home");
            }

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

        [HttpGet]
        public async Task<IActionResult> StartStocktaking()
        {
            List<Solicit> model = await _solicitHelper.ListStartStocktakingAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> StartStocktakingCreate()
        {
            ViewData["SchoolId"] = new SelectList(await _schoolHelper.ComboStartStocktakingAsync(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartStocktakingCreate(SolicitAddAndEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user= await _userHelper.GetUserAsync(User.Identity.Name);

                Solicit solicit = new Solicit()
                {
                    DateOfApprovedDenied = model.DateOfSolicit,
                    DateOfClosed = model.DateOfClosed,
                    DateOfReceived = model.DateOfSolicit,
                    DateOfSolicit = model.DateOfSolicit,
                    Description = model.Description,
                    School = await _schoolHelper.ByIdAsync(model.SchoolId),
                    SolicitStates = await _solicitStateHelper.ByIdAsync(TypeSolicitState.Inicial.ToString()),
                    UserApprovedDenied = user,
                    UserClosed=user,
                    UserReceived=user,
                };

                Response response = await _solicitHelper.AddUpdateAsync(solicit);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(StartStocktaking));
                }
            }

            ViewData["SchoolId"] = new SelectList(await _schoolHelper.ComboStartStocktakingAsync(), "Id", "Name",model.SchoolId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> StartStocktakingEdit(int id)
        {

            Solicit solicit = await _solicitHelper.ByIdAllAsync((int)id);

            if (solicit == null)
            {
                return NotFound();
            }

            SolicitAddAndEditViewModel model = new SolicitAddAndEditViewModel()
            {
                DateOfApprovedDenied=solicit.DateOfApprovedDenied,
                DateOfClosed=solicit.DateOfClosed,
                DateOfReceived=solicit.DateOfReceived,
                DateOfSolicit=solicit.DateOfSolicit,
                Description=solicit.Description,
                Id=solicit.Id,
                SchoolId=solicit.School.Id,
                SolicitStatesId=solicit.SolicitStates.Id,
            };

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartStocktakingEdit(SolicitAddAndEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                Solicit solicit = await _solicitHelper.ByIdAsync(model.Id);

                solicit.DateOfSolicit = model.DateOfSolicit;
                solicit.DateOfClosed = model.DateOfClosed;
                solicit.Description = model.Description;

                Response response=await _solicitHelper.AddUpdateAsync(solicit);

                if(response.IsSuccess)
                {
                    return RedirectToAction(nameof(StartStocktaking));
                }
            }


            return View(model);

        }

        [NoDirectAccess]
        public async Task<IActionResult> StartStocktakingDelete(int id)
        {
            Response response = await _solicitHelper.DeleteAsync(id);

            TempData["AlertMessage"] = response.Message;

            return RedirectToAction(nameof(StartStocktaking));
        }

        [HttpGet]
        public async Task<IActionResult> StartStocktakingDetails(int id)
        {
            Solicit model=await _solicitHelper.ByIdDetailAsync(id);

            if (model==null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> StartStocktakingProductAdd(int id)
        {
            SolicitDetailProductViewModel model = new SolicitDetailProductViewModel()
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
        public async Task<IActionResult> StartStocktakingProductAdd(SolicitDetailProductViewModel model)
        {
            if (ModelState.IsValid)
            {

                SolicitDetail sd = await _solicitDetailHelper.ByIdAsync(model.Id, model.ProductId);

                if (sd != null)
                {
                    sd.Quantity += model.Quantity;
                }
                else
                {
                    Solicit solicit = await _solicitHelper.ByIdAsync(model.Id);

                    Product product = await _productHelper.ByIdAsync(model.ProductId);

                    ApplicationUser user = await _userHelper.GetUserAsync(User.Identity.Name);

                    sd = new SolicitDetail()
                    {
                        Solicit = solicit,
                        Product = product,
                        Quantity = model.Quantity,
                        DirectorQuantity = model.Quantity,
                        PlannerQuantity = model.Quantity,
                        DeliveredQuantity = model.Quantity,
                        DateOfClosed = DateTime.Now,
                        UserDelivered = user,
                        Description = "s/d"
                    };
                }

                Response response = await _solicitDetailHelper.AddUpdateAsync(sd);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(StartStocktakingDetails), new { Id = model.Id });
                }

            }

            ViewData["CategoryTypeId"] = new SelectList(await _categoryTypeHelper.ComboAsync(), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(await _categoryHelper.ComboAsync(model.CategoryTypeId), "Id", "Name", model.CategoryTypeId);
            ViewData["ProductId"] = new SelectList(await _productHelper.ComboAsync(model.CategoryId), "Id", "Name", model.CategoryId);

            return View(model);
        }

        public async Task<IActionResult> StartStocktakingProductDelete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SolicitDetail model = await _solicitDetailHelper.ByIdAsync((int)id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("StartStocktakingProductDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartStocktakingProductDeleteConfirmed(int id)
        {
            SolicitDetail model = await _solicitDetailHelper.ByIdAsync((int)id);

            Response response = await _solicitDetailHelper.DeleteAsync((int)id);

            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(StartStocktakingDetails), new { id = model.Solicit.Id });
            }

            return View(model);
        }


        public async Task<IActionResult> Create()
        {
            string lnaem = await _userHelper.ByNameUneRoleAsync(User.Identity.Name);

            ApplicationUser user = await _userHelper.GetUserAsync(User.Identity.Name);
            List<SolicitState> state = await _solicitStateHelper.SolicitudStateAsync(lnaem, true);

            SolicitAddAndEditViewModel model = new SolicitAddAndEditViewModel()
            {
                SchoolId=2,
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

            if (model == null)
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

            solicit.SolicitStates=await _solicitStateHelper.ByNotInitialAsync(TypeSolicitState.Pendiente.ToString());

            ApplicationUser user = await _userHelper.GetUserAsync(User.Identity.Name);

            solicit.UserReceived = user;

            solicit.DateOfReceived = DateTime.Now;

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

            solicit.SolicitStates = await _solicitStateHelper.ByNotInitialAsync(TypeSolicitState.Consolidado.ToString());

            ApplicationUser user = await _userHelper.GetUserAsync(User.Identity.Name);

            solicit.UserApprovedDenied = user;

            solicit.DateOfApprovedDenied = DateTime.Now;

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

            solicit.SolicitStates = await _solicitStateHelper.ByNotInitialAsync(TypeSolicitState.Denegado.ToString());

            ApplicationUser user = await _userHelper.GetUserAsync(User.Identity.Name);

            solicit.UserApprovedDenied = user;

            solicit.DateOfApprovedDenied=DateTime.Now;

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
                sd.PlannerQuantity = model.Quantity;
                sd.DeliveredQuantity = model.Quantity;
                if (model.Description != null && model.Description != "s/d" && model.Description.Trim().Length > 3)
                {
                    sd.Description += $"\n Rector : {model.Description.Trim()}";
                }

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
                Response response = await _solicitHelper.AddUpdateAsync(User.Identity.Name,model.Description);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SolicitProcessing(int id)
        {
            Solicit solicit = await _solicitHelper.ByIdAsync(id);

            return View(solicit);
        }

        [HttpPost, ActionName("SolicitProcessing")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitProcessingConfirmed(int id)
        {
            Solicit solicit = await _solicitHelper.ByIdAsync(id);

            solicit.SolicitStates =await _solicitStateHelper.ByNotInitialAsync(TypeSolicitState.Proceso.ToString());
            solicit.UserReceived = await _userHelper.ByIdAsync(User.Identity.Name);
            solicit.DateOfReceived = DateTime.Now;

            Response response = await _solicitHelper.AddUpdateAsync(solicit);

            TempData["AlertMessage"] = response.Message;
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> PlannerEditQuantity(int id)
        {
            SolicitDetail model = await _solicitDetailHelper.ByIdAsync(id);

            ProductEditrectorPlanner p = new ProductEditrectorPlanner()
            {
                Id = model.Id,
                Name = model.Product.Name,
                Description = model.Description,
                Quantity = model.PlannerQuantity,
                SolicitId = model.Solicit.Id
            };

            return View(p);
        }

        [HttpPost, ActionName("PlannerEditQuantity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlannerEditQuantityConfirmed([Bind("Id,SolicitId,Name,Description,Quantity")] ProductEditrectorPlanner model)
        {
            if (ModelState.IsValid)
            {
                SolicitDetail sd = await _solicitDetailHelper.ByIdAsync(model.Id);

                sd.PlannerQuantity = model.Quantity;
                sd.DeliveredQuantity = model.Quantity;

                if (model.Description != "s/d" && model.Description.Trim().Length > 3)
                {
                    sd.Description += $"\n Planificador : {model.Description.Trim()}";
                }

                Response response = await _solicitDetailHelper.AddUpdateAsync(sd);

                TempData["AlertMessage"] = response.Message;

                return RedirectToAction(nameof(Details), new { id = model.SolicitId });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SolicitClosed(int id)
        {
            Solicit solicit = await _solicitHelper.ByIdAsync(id);

            return View(solicit);

        }

        [HttpPost, ActionName("SolicitClosed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitClosedConfirmed(int id)
        {
            Solicit solicit = await _solicitHelper.ByIdAsync(id);

            solicit.SolicitStates = await _solicitStateHelper.ByNotInitialAsync(TypeSolicitState.Cerrado.ToString());

            solicit.UserClosed = await _userHelper.GetUserAsync(User.Identity.Name);

            solicit.DateOfClosed = DateTime.Now;

            Response response = await _solicitHelper.AddUpdateAsync(solicit);

            TempData["AlertMessage"] = response.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SolicitPlannerPassed(int id)
        {
            Solicit model = await _solicitHelper.ByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("SolicitPlannerPassed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitPlannerPassedConfirmed(int id)
        {
            Solicit solicit = await _solicitHelper.ByIdAsync(id);

            solicit.SolicitStates = await _solicitStateHelper.ByNotInitialAsync(TypeSolicitState.Aceptado.ToString());

            Response response = await _solicitHelper.AddUpdateAsync(solicit);

            TempData["AlertMessage"] = response.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeliveredEditQuantity(int id)
        {
            SolicitDetail model = await _solicitDetailHelper.ByIdAsync(id);

            ProductEditrectorPlanner p = new ProductEditrectorPlanner()
            {
                Id = model.Id,
                Name = model.Product.Name,
                Description = model.Description,
                Quantity = model.DeliveredQuantity,
                SolicitId = model.Solicit.Id
            };

            return View(p);
        }

        [HttpPost, ActionName("DeliveredEditQuantity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeliveredEditQuantity([Bind("Id,SolicitId,Name,Description,Quantity")] ProductEditrectorPlanner model)
        {
            if (ModelState.IsValid)
            {
                SolicitDetail sd = await _solicitDetailHelper.ByIdAsync(model.Id);

                sd.DeliveredQuantity = model.Quantity;

                if(model.Description != "s/d" && model.Description.Trim().Length>3)
                {
                    sd.Description +=$"\nEntregado : {model.Description.Trim()}";
                }

                Response response = await _solicitDetailHelper.AddUpdateAsync(sd);

                TempData["AlertMessage"] = response.Message;

                return RedirectToAction(nameof(Details), new { id = model.SolicitId });
            }

            return View(model);
        }
              
        public async Task<IActionResult> SolicitPrint(int id)
        {
            MemoryStream ms = await _pdfDocument.ReportSoliAsync(id);

            return File(ms.ToArray(), "application/pdf");
        }

        public async Task<IActionResult> SolicitReport()
        {
            ViewData["CityId"] = new SelectList(await _cityHelper.ComboAsync(1), "Id", "Name");
            ViewData["SchoolId"] = new SelectList(await _schoolHelper.ComboCityAsync(0,true), "Id", "Name");
            ViewData["CampusId"] = new SelectList(await _schoolHelper.ComboCityAsync(0), "Id", "Name");
            ViewData["CategoryTypeId"] = new SelectList(await _categoryTypeHelper.ComboAsync(), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(await _categoryHelper.ComboAsync(0), "Id", "Name");
            ViewData["ProductId"] = new SelectList(await _productHelper.ComboAsync(0), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitReport(SolicitReportViewModel model)
        {
            MemoryStream ms = await _pdfDocument.ReportProductConsolidatedAsync(model);

            return File(ms.ToArray(), "application/pdf");
        }


        [HttpGet]
        public async Task<IActionResult> GeneListSchools(int Id,bool IsEsta=false)
        {
            List<School> lista = await _schoolHelper.ComboCityAsync(Id, IsEsta);

            return Json(lista);
        }

    }
}
