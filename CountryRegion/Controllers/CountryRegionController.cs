using Elvodora.Fields.CountryRegion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OrchardCore.ContentManagement;

namespace Elvodora.Fields.CountryRegion.Controllers
{
    [ApiController]
    public class CountryRegionController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICountryRegionService _service;

        public CountryRegionController(
            IContentManager contentManager,
            IAuthorizationService authorizationService,
            ICountryRegionService service
            )
        {
            _contentManager = contentManager;
            _authorizationService = authorizationService;
            _service = service;
        }
        [HttpGet]
        [Route("api/elvodora/countries")]        
        public IActionResult GetCountries()
        {
            return Ok(_service.GetAllCountriesWithoutRegions());
        }
        [HttpGet]
        [Route("api/elvodora/regions/{code}")]
        public IActionResult GetRegions(string code)
        {
            if (code == null)
            {
                return NotFound();               
            }
            var country = _service.GetCountryWithRegions(code);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country.Regions); 
        }
    }
}
