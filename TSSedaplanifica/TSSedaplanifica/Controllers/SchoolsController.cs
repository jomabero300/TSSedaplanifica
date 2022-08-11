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
    [Authorize(Roles = $"{nameof(TypeUser.Administrador)}")]

    public class SchoolsController : Controller
    {
        private readonly ISchoolHelper _schoolHelper;
        private readonly ICityHelper _cityHelper;
        private readonly IZoneHelper _zoneHelper;
        private readonly IUserHelper _userHelper;
        private readonly ISchoolUserHelper _schoolUserHelper;

        public SchoolsController(ISchoolHelper schoolHelper, ICityHelper cityHelper, IZoneHelper zoneHelper, IUserHelper userHelper, ISchoolUserHelper schoolUserHelper)
        {
            _schoolHelper = schoolHelper;
            _cityHelper = cityHelper;
            _zoneHelper = zoneHelper;
            _userHelper = userHelper;
            _schoolUserHelper = schoolUserHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _schoolHelper.ListAsync());
        }

        // GET: CategoryTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            School model = await _schoolHelper.ByIdAsync((int)id);

            if (model == null)
            {
                return NotFound();
            }

            List<School> campus = await _schoolHelper.ListAsync(model.Id);

            SchoolDetailsViewModel vm = new SchoolDetailsViewModel()
            {
                Id=model.Id,
                Address=model.Address,
                DaneCode=model.DaneCode,
                Name=model.Name,
                City=model.City,
                Zone=model.Zone,                
                Schools=campus
            };

            return View(vm);
        }

        // GET: CategoryTypes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CityId"] = new SelectList(await _cityHelper.ComboAsync(1), "Id", "Name");

            ViewData["ZoneId"] = new SelectList(await _zoneHelper.ComboAsync(), "Id", "Name");

            return View();
        }

        // POST: CategoryTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DaneCode,Address,CityId,ZoneId")] SchoolViewModel model)
        {
            if (ModelState.IsValid)
            {
                City city = await _cityHelper.ByIdAsync(model.CityId);

                Zone zone = await _zoneHelper.ByIdAsync(model.ZoneId);

                Response response = await _schoolHelper.AddUpdateAsync(
                    new School()
                    {
                        Name = model.Name,
                        DaneCode = model.DaneCode,
                        Address = model.Address,
                        City = city,
                        Zone = zone,
                    });

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ViewData["CityId"] = new SelectList(await _cityHelper.ComboAsync(1), "Id", "Name", model.CityId);

            ViewData["ZoneId"] = new SelectList(await _zoneHelper.ComboAsync(), "Id", "Name", model.ZoneId);

            return View(model);
        }

        // GET: CategoryTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            School school = await _schoolHelper.ByIdAsync((int)id);

            if (school == null)
            {
                return NotFound();
            }

            SchoolViewModel model = new SchoolViewModel
            {
                Id = school.Id,
                Name = school.Name,
                Address = school.Address,
                DaneCode = school.DaneCode,
                CityId = school.City.Id,
                ZoneId = school.Zone.Id,
            };

            ViewData["CityId"] = new SelectList(await _cityHelper.ComboAsync(1), "Id", "Name", model.CityId);

            ViewData["ZoneId"] = new SelectList(await _zoneHelper.ComboAsync(), "Id", "Name", model.ZoneId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DaneCode,Address,CityId,ZoneId")] SchoolViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                City city = await _cityHelper.ByIdAsync(model.CityId);

                Zone zone = await _zoneHelper.ByIdAsync(model.ZoneId);

                School sc = new School()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Address = model.Address,
                    DaneCode = model.DaneCode,
                    City = city,
                    Zone = zone,
                };

                Response response = await _schoolHelper.AddUpdateAsync(sc);

                TempData["AlertMessage"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ViewData["CityId"] = new SelectList(await _cityHelper.ComboAsync(1), "Id", "Name", model.CityId);

            ViewData["ZoneId"] = new SelectList(await _zoneHelper.ComboAsync(), "Id", "Name", model.ZoneId);

            return View(model);
        }

        // GET: CategoryTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            School model = await _schoolHelper.ByIdAsync((int)id);

            return View(model);
        }

        // POST: CategoryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Response response = await _schoolHelper.DeleteAsync((int)id);


            if (response.IsSuccess)
            {
                TempData["AlertMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            School model = await _schoolHelper.ByIdAsync((int)id);

            ModelState.AddModelError(string.Empty, response.Message);


            return View(model);
        }

        public async Task< IActionResult> AddSchool(int id)
        {

            School modelI = await _schoolHelper.ByIdAsync((int)id);

            SchoolViewModel model = new SchoolViewModel()
            {
                Id =modelI.Id,
                CityId = modelI.City.Id,
                ZoneId=modelI.Zone.Id,
            };

            ViewData["CityId"] = new SelectList(await _cityHelper.ComboAsync(1), "Id", "Name");

            ViewData["ZoneId"] = new SelectList(await _zoneHelper.ComboAsync(), "Id", "Name");


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSchool([Bind("Id,Name,DaneCode,Address,CityId,ZoneId,SchoolCampus")] SchoolViewModel model)
        {
            if(ModelState.IsValid)
            {
                City city =await _cityHelper.ByIdAsync(model.CityId);
                Zone zone =await _zoneHelper.ByIdAsync(model.ZoneId);
                School institución = await _schoolHelper.ByIdAsync(model.Id);

                School sc = new School()
                {
                    Name = model.Name,
                    DaneCode = model.DaneCode,
                    Address = model.Address,
                    City=city,
                    Zone=zone,
                    SchoolCampus=institución
                };

                Response response = await _schoolHelper.AddUpdateAsync(sc);


                if (response.IsSuccess)
                {
                    TempData["AlertMessage"] = response.Message.Replace("Institución","Seda");

                    return RedirectToAction(nameof(Details),new { id= institución.Id});
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ViewData["CityId"] = new SelectList(await _cityHelper.ComboAsync(1), "Id", "Name",model.CityId);

            ViewData["ZoneId"] = new SelectList(await _zoneHelper.ComboAsync(), "Id", "Name", model.ZoneId);


            return View(model);
        }
        
        public async Task<IActionResult>  DeleteSchool(int id)
        {
            School model = await _schoolHelper.ByIdAsync(id);

            if(model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("DeleteSchool")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSchoolConfirmed(School model)
        {
            School modelI = await _schoolHelper.ByIdAsync(model.Id);

            Response response = await _schoolHelper.DeleteAsync(model.Id);
            
            TempData["AlertMessage"] = response.Message.Replace("Institución", "Sede");

            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(Details), new { id = modelI.SchoolCampus.Id });
            }

            ModelState.AddModelError(string.Empty, response.Message);

            return View(model);
        }

        public async Task<IActionResult> Rector(int id)
        {
            string typeUser = TypeUser.Rector.ToString();

            if (id >= 68 && id <= 74)
            {
                typeUser = TypeUser.Secretario_municipal.ToString();
            }
            var rol = await _userHelper.ByIdRoleAsync(typeUser);

            var schoolName = await _schoolHelper.ByIdAsync(id);

            SchoolRectorCoordinator model = new SchoolRectorCoordinator()
            {
                SchoolId = id,
                rol = rol.Id,
                SchoolName = schoolName.Name,
                SchoolCampus=1,
                assignSeat=false
            };

            ViewData["UserId"] = new SelectList(await _userHelper.ListUserNotAssignedAsync(typeUser), "Id", "FullName");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rector(SchoolRectorCoordinator model)
        {
            if (ModelState.IsValid)
            {

                if(model.HireOfDate.Year<DateTime.Now.Date.Year)
                {
                    ModelState.AddModelError(string.Empty, "La fecha inicial no se encuentra dentro de este año");
                }
                else if (model.HireOfDate>model.EndOfDate)
                {
                    ModelState.AddModelError(string.Empty, "La fecha inicial NO debe ser superior a la fecha final");
                }
                else
                {
                    Response response= await _schoolUserHelper.AddUpdateAsync(model);

                    if (response.IsSuccess)
                    {
                        TempData["AlertMessage"] = response.Message;

                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            ViewData["UserId"] = new SelectList(await _userHelper.ListUserNotAssignedAsync(TypeUser.Rector.ToString()), "Id", "FullName",model.UserId);

            return View(model);
        }

        public async Task<IActionResult> Coordinator(int id)
        {
            var rol = await _userHelper.ByIdRoleAsync(TypeUser.Coordinador.ToString());

            School schoolName = await _schoolHelper.ByIdAsync(id);

            SchoolRectorCoordinator model = new SchoolRectorCoordinator()
            {
                SchoolId = id,
                rol = rol.Id,
                SchoolName = schoolName.Name,
                SchoolCampus=schoolName.SchoolCampus.Id,
                assignSeat=true
            };

            ViewData["UserId"] = new SelectList(await _userHelper.ListUserNotAssignedAsync(schoolName.SchoolCampus.Id), "Id", "FullName");


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Coordinator(SchoolRectorCoordinator model)
        {
            if (ModelState.IsValid)
            {
                if(model.HireOfDate<DateTime.Now.Date)
                {
                    ModelState.AddModelError(string.Empty, "La fecha inicial debe ser igual o superior a la fecha de hoy");
                }
                else if (model.HireOfDate>model.EndOfDate)
                {
                    ModelState.AddModelError(string.Empty, "La fecha inicial NO debe ser superior a la fecha final");
                }
                else
                {
                    Response response= await _schoolUserHelper.AddUpdateAsync(model);

                    if (response.IsSuccess)
                    {
                        TempData["AlertMessage"] = response.Message;

                        return RedirectToAction(nameof(Details), new { id = model.SchoolCampus });
                    }
                }
            }

            ViewData["UserId"] = new SelectList(await _userHelper.ListUserNotAssignedAsync(model.SchoolCampus), "Id", "FullName");

            ViewData["SchoolCampus"] = new SelectList(await _schoolHelper.ComboAsync(model.SchoolId), "Id", "Name", model.SchoolCampus);

            return View(model);
        }

        [NoDirectAccess]
        public async Task<IActionResult> CoordinatorDelete(int id)
        {

            Response response= await _schoolUserHelper.DeleteAsync(id);

            TempData["AlertMessage"] = response.Message;

            return RedirectToAction(nameof(Details), new {id=response.Result });
        }
    }
}
