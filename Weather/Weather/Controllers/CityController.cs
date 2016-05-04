using System.Threading.Tasks;
using System.Web.Http;
using Weather.Service;

namespace Weather.Controllers
{
    public class CityController : ApiController
    {
        private readonly IWeatherService service;

        public CityController(IWeatherService service)
        {
            this.service = service;
        }

        [Route("{countryName}/cities")]
        public async Task<IHttpActionResult> Get(string countryName)
        {
            var cities = await service.GetCitiesByCountry(countryName);

            return Ok(cities);
        }
    }
}
