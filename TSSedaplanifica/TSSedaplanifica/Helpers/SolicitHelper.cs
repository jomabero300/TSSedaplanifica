using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Models;

namespace TSSedaplanifica.Helpers
{
    public class SolicitHelper : ISolicitHelper
    {
        private const string _name = "Solicitud";

        private readonly ApplicationDbContext _context;

        public SolicitHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(Solicit model)
        {
            Response response = new Response() { IsSuccess = true };

            if (model.Id == 0)
            {
                _context.Solicits.Add(model);

                response.Message = $"{_name} guardado satisfactoriamente.!!!";
            }
            else
            {
                _context.Solicits.Update(model);

                response.Message = $"{_name} actualizado satisfactoriamente.!!!";
            }


            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplica"))
                {
                    response.Message = $"Ya existe una {_name} con el mismo nombre.";
                }
                else
                {
                    response.Message = dbUpdateException.InnerException.Message;
                }

                response.IsSuccess = false;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;

                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<Response> AddUpdateAsync(string id, string description,string username)
        {
            SolicitConsolidateViewModel model = await lmConsolidar(id);

            Response response = new Response() { IsSuccess = true };

            ApplicationUser userDelivere = await _context.Users.Where(u => u.Email == username).FirstOrDefaultAsync();

            ICollection<SolicitDetail> detalles = model.Details.Select(d => new SolicitDetail()
            {
                Product=d.Product,
                Quantity=d.Quantity,
                DirectorQuantity=d.Quantity,
                PlannerQuantity=d.Quantity,
                DeliveredQuantity=d.Quantity,
                Description="s/d",
                DateOfClosed=DateTime.Now,
                UserDelivered=userDelivere,

            }).ToList();

            Solicit solicit = new Solicit() { 
                School=model.School,
                DateOfSolicit   =DateTime.Now,
                Description = description,
                SolicitStates=await _context.SolicitStates.Where(s=>s.Name==TypeSolicitState.Enviado.ToString()).FirstOrDefaultAsync(),
                DateOfReceived = DateTime.Now,
                UserReceived= userDelivere,
                DateOfApprovedDenied = DateTime.Now,
                UserApprovedDenied= userDelivere,
                DateOfClosed = DateTime.Now,
                SolicitDetails= detalles,
                UserClosed= userDelivere
            };
            List<int> solicitIds = model.SolicitCons.Select(s=>s.id).ToList();

            List<Solicit> solicitUpdate = _context.Solicits.Where(s => solicitIds.Contains(s.Id)).ToList();

            SolicitState solicitState = await _context.SolicitStates.Where(s => s.Name == TypeSolicitState.Aceptado.ToString()).FirstOrDefaultAsync();

            solicitUpdate.ForEach(s => s.SolicitStates = solicitState);

            try
            {
                _context.Solicits.Add(solicit);

                await _context.SaveChangesAsync();

                response.Message = $"{_name} guardado satisfactoriamente.!!!";

                solicitUpdate.ForEach(s => s.SolicitReferred = solicit);

                _context.Solicits.UpdateRange(solicitUpdate);

                await _context.SaveChangesAsync();



            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplica"))
                {
                    response.Message = $"Ya existe una {_name} con el mismo nombre.";
                }
                else
                {
                    response.Message = dbUpdateException.InnerException.Message;
                }

                response.IsSuccess = false;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;

                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<Solicit> ByIdAsync(int id)
        {
            return await _context.Solicits
                                .Where(s=>s.Id==id)
                                .FirstOrDefaultAsync();
        }

        public async Task<Solicit> ByIdDetailAsync(int id)
        {
            Solicit model = await _context.Solicits
                                .Include(a=>a.School).ThenInclude(w=>w.SchoolCampus)
                                .Include(s => s.SolicitStates)
                                .Include(s => s.SolicitDetails)
                                .ThenInclude(x=>x.Product)
                                .Where(s => s.Id == id)
                                .FirstOrDefaultAsync();
            return model;
        }

        public Task<List<Solicit>> ComboAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true, Message= $"{_name} borrado satisfactoriamente.!!!" };

            Solicit model =await _context.Solicits.Include(s=>s.SolicitDetails).Where(x=>x.Id==id).FirstOrDefaultAsync();

            try
            {
                _context.Solicits.Remove(model);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplica"))
                {
                    response.Message = $"Ya existe una {_name} con el mismo nombre.";
                }
                else
                {
                    response.Message = dbUpdateException.InnerException.Message;
                }

                response.IsSuccess = false;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;

                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<List<SolicitViewModel>> ListAsync(string id)
        {
            List<Solicit> solicits=new  List<Solicit>();
            List<Solicit> solicitsO = new List<Solicit>(); ;

            var user = await _context.UserRoles.Where(u => u.UserId == id).FirstOrDefaultAsync();
            var rol = await _context.Roles.FindAsync(user.RoleId);
            
            SchoolUser su = await _context.SchoolUsers
                                                .Include(s => s.School)
                                                .Where(s=>s.ApplicationUser.Id == id && s.isEnable == true)
                                                .FirstOrDefaultAsync();

            if (rol.Name == TypeUser.Rector.ToString())
            {
                solicitsO = await _context.Solicits
                                        .Include(s => s.SolicitStates)
                                        .Include(s => s.UserReceived)
                                        .Include(s => s.UserApprovedDenied)
                                        .Include(s => s.UserClosed)
                                        .Include(s => s.School).ThenInclude(x => x.SchoolUsers)
                                        .Include(d=>d.SolicitDetails)
                                        .Where(s => s.School.SchoolCampus.Id == su.School.Id &&
                                                    s.SolicitStates.Name == TypeSolicitState.Enviado.ToString() ||
                                                    s.SolicitStates.Name == TypeSolicitState.Consolidado.ToString() ||
                                                    s.SolicitStates.Name == TypeSolicitState.Aceptado.ToString() ||
                                                    s.SolicitStates.Name == TypeSolicitState.Pendiente.ToString())
                                        .ToListAsync();
            }

            List<Solicit> solicits1 = await _context.Solicits
                                .Include(s => s.SolicitStates)
                                .Include(s => s.School).ThenInclude(x => x.SchoolUsers)
                                .Include(d => d.SolicitDetails)
                                .Where(s => s.School.Id == su.School.Id && !solicitsO.Contains(s))
                                .ToListAsync();

            List<SolicitViewModel> model = solicits1.Select(x => new SolicitViewModel()
            {
                Id=x.Id,
                School=x.School,
                DateOfSolicit=x.DateOfSolicit,
                Description=x.Description,
                SolicitStates=x.SolicitStates,
                DateOfReceived=x.DateOfReceived,
                UserReceived=x.UserReceived,
                DateOfApprovedDenied=x.DateOfApprovedDenied,
                UserApprovedDenied=x.UserApprovedDenied,
                DateOfClosed=x.DateOfClosed,
                UserClosed=x.UserClosed,
                TypeUser=true,
                SolicitDetails=x.SolicitDetails
            }).ToList();

            if (solicitsO.Count() >0 )
            {
                foreach (var item in solicitsO)
                {
                    model.Add(new SolicitViewModel()
                    {
                        Id = item.Id,
                        School = item.School,
                        DateOfSolicit = item.DateOfSolicit,
                        Description = item.Description,
                        SolicitStates = item.SolicitStates,
                        DateOfReceived = item.DateOfReceived,
                        UserReceived = item.UserReceived,
                        DateOfApprovedDenied = item.DateOfApprovedDenied,
                        UserApprovedDenied = item.UserApprovedDenied,
                        DateOfClosed = item.DateOfClosed,
                        UserClosed = item.UserClosed,
                        TypeUser = false,
                        SolicitDetails=item.SolicitDetails
                    });
                }
            }

            return model;
        }

        public async Task<SolicitConsolidateViewModel> ListConsolidateAsync(string id)
        {
            SolicitConsolidateViewModel model=await lmConsolidar(id);

            return model;
        }

        public async Task<Response> RequestSendAsync(int id, string typeSolicitState)
        {
            Response response = new Response() { IsSuccess = false };

            int solicitudDetailCount = await _context.SolicitDetails.Where(s => s.Solicit.Id == id).CountAsync();

            if(solicitudDetailCount > 0)
            {
                Solicit solicit=await _context.Solicits.FindAsync(id);

                solicit.SolicitStates = await _context.SolicitStates.Where(s => s.Name == typeSolicitState).FirstOrDefaultAsync();

                try
                {
                    _context.Update(solicit);

                    await _context.SaveChangesAsync();

                    response.Message = $"{_name} enviada exitosamente. !!!";
                    response.IsSuccess = true;
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplica"))
                    {
                        response.Message = $"Ya existe una {_name} con el mismo nombre.";
                    }
                    else
                    {
                        response.Message = dbUpdateException.InnerException.Message;
                    }

                    response.IsSuccess = false;
                }
                catch (Exception exception)
                {
                    response.Message = exception.Message;

                    response.IsSuccess = false;
                }
            }
            else
            {
                response.Message = $"La {_name} NO tiene elementos a solicitar...";
            }

            return response;
        }

        private async Task<SolicitConsolidateViewModel> lmConsolidar(string id)
        {
            School school = await _context.SchoolUsers
                                              .Include(s => s.School)
                                              .Where(s => s.ApplicationUser.Email == id && s.isEnable == true)
                                              .Select(s => s.School)
                                              .FirstOrDefaultAsync();

            List<int> schoolsId = await _context.Schools.Where(s => s.SchoolCampus.Id == school.Id)
                                              .Select(s => s.Id).ToListAsync();

            schoolsId.Add(school.Id);

            List<SolicitDetail> details = await _context.SolicitDetails
                                .Include(p => p.Product)
                                .Include(s => s.Solicit)
                                .ThenInclude(x => x.School)
                                .Where(s => s.Solicit.SolicitStates.Name == TypeSolicitState.Consolidado.ToString() &&
                                        schoolsId.Contains(s.Solicit.School.Id))
                                .ToListAsync();

            var detalles = from detail in details
                           group detail by detail.Product into resulDetail
                           select new { resulDetail.Key, TotlaQuantity = resulDetail.Sum(r => r.DirectorQuantity) };


            var solicitId = details.GroupBy(m => m.Solicit.Id).Select(x => new { codigo = x.Key });



            SolicitConsolidateViewModel modelGroup = new SolicitConsolidateViewModel();

            foreach (var item in solicitId)
            {
                modelGroup.SolicitCons.Add(new SolicitConso { id = item.codigo });
            }


            modelGroup.School = school;

            foreach (var item in detalles)
            {
                Product pro = await _context.Products
                                            .Include(x => x.MeasureUnit)
                                            .Include(s => s.ProductCategories)
                                            .Where(s => s.Id == item.Key.Id).FirstOrDefaultAsync();

                modelGroup.Details.Add(new SolicitConsolidateDetailsViewModel { Product = pro, Quantity = (int)item.TotlaQuantity });
            }

            return modelGroup;
        }
    }
}
