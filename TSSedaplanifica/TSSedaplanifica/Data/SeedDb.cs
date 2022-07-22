using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Helpers;

namespace TSSedaplanifica.Data
{
    public class SeedDb
    {
        private readonly ApplicationDbContext _context;
        private readonly IApiService _apiService;

        public SeedDb(ApplicationDbContext context, IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            //await CheckCategoriesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Tecnología" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Gamer" });
                _context.Categories.Add(new Category { Name = "Belleza" });
                _context.Categories.Add(new Category { Name = "Nutrición" });
            }

            await _context.SaveChangesAsync();
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
    }
}
