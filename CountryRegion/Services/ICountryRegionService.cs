using Elvodora.Fields.CountryRegion.Models;
using System.Collections.Generic;

namespace Elvodora.Fields.CountryRegion.Services
{
    public interface ICountryRegionService
    {
        public IList<Country> GetAllCountriesWithRegions();
        public IList<Country> GetAllCountriesWithoutRegions();
        public Country GetCountryWithRegions(string countryCode);
    }
}
