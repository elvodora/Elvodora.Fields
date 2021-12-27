using Elvodora.Fields.CountryRegion.Data;
using Elvodora.Fields.CountryRegion.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Elvodora.Fields.CountryRegion.Services
{
    public class CountryRegionService : ICountryRegionService
    {
        private IList<Country> Countries;
        public CountryRegionService()
        {
            LoadCountries();
        }
        public IList<Country> GetAllCountriesWithRegions()
        {
            var countries = Countries.Select(country => (new Country { Name = country.Name, Code = country.Code, Regions = country.Regions })).ToList();
            return countries;
        }

        public IList<Country> GetAllCountriesWithoutRegions()
        {
            var countries = Countries.Select(country => (new Country { Name = country.Name, Code = country.Code })).ToList();
            return countries;
        }

        public Country GetCountryWithRegions(string countryCode)
        {
            var country = Countries.FirstOrDefault(country => country.Code == countryCode);
            return country;
        }

        private void LoadCountries()
        {
            Countries = JsonSerializer.Deserialize<IList<Country>>(Constants.CountriesJson);
        }

    }
}
