using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Enum;
using TSSedaplanifica.Helpers;

namespace TSSedaplanifica.Data
{
    public class SeedDb
    {
        private readonly ApplicationDbContext _context;
        private readonly IApiService _apiService;
        private readonly IUserHelper _userHelper;


        public SeedDb(ApplicationDbContext context, IApiService apiService, IUserHelper userHelper)
        {
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckZoneAsync();
            await CheckCountriesAsync();
            await CheckCategoryTypeAsync();
            await CheckCategoryTypeDesAsync();
            await CheckMeasureUnitAsync();
            await CheckSolicitStateAsync();

            await CheckRolesAsync();
            await CheckUserAsync("0000","admin@gmail.com","Super","administrador",Guid.Empty,"3000000000",TypeUser.Administrador);
            await CheckUserAsync("1010","jomabero300@gmail.com","Manuel","Bello",Guid.Empty,"313670740",TypeUser.Administrador);
            await CheckUserAsync("2020","leonardopulidom@gmail.com","Leonardo","Pulido",Guid.Empty,"3134907527",TypeUser.Administrador);

            //await CheckUserAsync("3030","GustavovillaRector@gmail.com","Rector","villa",Guid.Empty,"3134907527",TypeUser.Rector);
            //await CheckUserAsync("4040", "GustavovillaCoordinador@gmail.com", "Coordinador","villa",Guid.Empty,"3134907527",TypeUser.Coordinador);
        }

        private async Task<ApplicationUser> CheckUserAsync(
            string document,
            string email,
            string firstName,
            string lastName,
            Guid ImageId,
            string phone,
            TypeUser typeUser)
        {
            ApplicationUser user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phone,
                    Document = document,
                };

                await _userHelper.AddUserAsync(user, "User123.");
                await _userHelper.AddUserToRoleAsync(user, typeUser.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(TypeUser.Administrador.ToString());
            await _userHelper.CheckRoleAsync(TypeUser.Coordinador.ToString());
            await _userHelper.CheckRoleAsync(TypeUser.Planificador.ToString());
            await _userHelper.CheckRoleAsync(TypeUser.Rector.ToString());
            await _userHelper.CheckRoleAsync(TypeUser.Secretario_municipal.ToString());
        }

        private async Task CheckZoneAsync()
        {
            if (!_context.Zones.Any())
            {
                _context.Zones.Add(new Zone { Name = "Rural" });
                _context.Zones.Add(new Zone { Name = "Urbano" });

                await _context.SaveChangesAsync();
            }

        }

        private async Task CheckSolicitStateAsync()
        {
            if (!_context.SolicitStates.Any())
            {
                _context.SolicitStates.Add(new SolicitState { Name = TypeSolicitState.Aceptado.ToString() });
                _context.SolicitStates.Add(new SolicitState { Name = TypeSolicitState.Borrador.ToString() });
                _context.SolicitStates.Add(new SolicitState { Name = TypeSolicitState.Cerrado.ToString() });
                _context.SolicitStates.Add(new SolicitState { Name = TypeSolicitState.Consolidado.ToString() });
                _context.SolicitStates.Add(new SolicitState { Name = TypeSolicitState.Denegado.ToString() });
                _context.SolicitStates.Add(new SolicitState { Name = TypeSolicitState.Enviado.ToString() });
                _context.SolicitStates.Add(new SolicitState { Name = TypeSolicitState.Inicial.ToString() });
                _context.SolicitStates.Add(new SolicitState { Name = TypeSolicitState.Pendiente.ToString() });
                _context.SolicitStates.Add(new SolicitState { Name = TypeSolicitState.Proceso.ToString() });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckMeasureUnitAsync()
        {
            if (!_context.MeasureUnits.Any())
            {
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Cuñete",NameShort="Cuñe" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Galón",NameShort="Gal" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Gigabit",NameShort="Gb" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "GigaByte",NameShort="GB" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Global",NameShort="Gl" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Gramo",NameShort="G" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Kilogramo",NameShort="Kg" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Kilómetro",NameShort="Km" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Litro",NameShort="Lt" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Megabit",NameShort="Mb" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Megabyte",NameShort="MB" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "MegaByte por segundo",NameShort="Mb/s" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Metro",NameShort="M" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Metro cúbico",NameShort="M3" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Pulgada",NameShort="''" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "TeraByte",NameShort="TB" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "TeraByte por segundo",NameShort="Tb/s" });
                _context.MeasureUnits.Add(new MeasureUnit { Name = "Unidad",NameShort="Un" });
                

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                Response responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries");

                if (responseCountries.IsSuccess)
                {
                    List<CountryResponse> countries = (List<CountryResponse>)responseCountries.Result;
                    foreach (CountryResponse countryResponse in countries)
                    {
                        if (countryResponse.Name == "Colombia")
                        {


                            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Name == countryResponse.Name);
                            if (country == null)
                            {
                                country = new() { Name = countryResponse.Name, States = new List<State>() };

                                Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
                                if (responseStates.IsSuccess)
                                {
                                    List<StateResponse> states = (List<StateResponse>)responseStates.Result;
                                    foreach (StateResponse stateResponse in states)
                                    {
                                        if (stateResponse.Name == "Arauca")
                                        {


                                            State state = country.States.FirstOrDefault(s => s.Name == stateResponse.Name);
                                            if (state == null)
                                            {
                                                state = new() { Name = stateResponse.Name, Cities = new List<City>() };
                                                Response responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                                                if (responseCities.IsSuccess)
                                                {
                                                    List<CityResponse> cities = (List<CityResponse>)responseCities.Result;
                                                    foreach (CityResponse cityResponse in cities)
                                                    {
                                                        if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
                                                        {
                                                            continue;
                                                        }
                                                        City city = state.Cities.FirstOrDefault(c => c.Name == cityResponse.Name);
                                                        if (city == null)
                                                        {
                                                            state.Cities.Add(new City() { Name = cityResponse.Name });
                                                        }
                                                    }
                                                }
                                                if (state.CityNumber > 0)
                                                {
                                                    country.States.Add(state);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (country.CitiesNumber > 0)
                                {
                                    _context.Countries.Add(country);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }

        }

        private async Task CheckCategoryTypeAsync()
        {
            if (!_context.CategoryTypes.Any())
            {
                _context.CategoryTypes.Add(new CategoryType { Name = "Infraestructura" });
                _context.CategoryTypes.Add(new CategoryType { Name = "Dotación" });
                _context.CategoryTypes.Add(new CategoryType { Name = "Bioseguridad" });
                await _context.SaveChangesAsync();
            }

        }

        private async Task CheckCategoryTypeDesAsync()
        {
            if(!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Agropecuario" });
                _context.Categories.Add(new Category { Name = "Ambiente" });
                _context.Categories.Add(new Category { Name = "Arte" });
                _context.Categories.Add(new Category { Name = "Biblioteca" });
                _context.Categories.Add(new Category { Name = "Conjunto" });
                _context.Categories.Add(new Category { Name = "Construcción" });
                _context.Categories.Add(new Category { Name = "Deportes" });
                _context.Categories.Add(new Category { Name = "Herramientas Pedagógicas" });
                _context.Categories.Add(new Category { Name = "Laboratorios" });
                _context.Categories.Add(new Category { Name = "Mejoramiento" });
                _context.Categories.Add(new Category { Name = "Menaje" });
                _context.Categories.Add(new Category { Name = "Mobiliario" });
                _context.Categories.Add(new Category { Name = "Salud" });
                _context.Categories.Add(new Category { Name = "Tecnología" });

                await _context.SaveChangesAsync();

                CategoryType ct = await _context.CategoryTypes.Where(c => c.Name == "Infraestructura").FirstOrDefaultAsync();

                if(ct != null)
                {
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Agropecuario").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Ambiente").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Arte").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Biblioteca").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Conjunto").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Construcción").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Deportes").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Herramientas Pedagógicas").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Laboratorios").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Mejoramiento").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Menaje").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Mobiliario").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Salud").FirstOrDefaultAsync() });
                    _context.CategoryTypeDers.Add(new CategoryTypeDer { CategoryType = ct, Category = await _context.Categories.Where(c => c.Name == "Tecnología").FirstOrDefaultAsync() });

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
