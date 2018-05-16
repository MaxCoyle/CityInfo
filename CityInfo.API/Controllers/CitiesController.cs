using CityInfo.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private readonly ICitiesRepository _citiesRepository;

        public CitiesController(ICitiesRepository cityInfoRepository)
        {
            _citiesRepository = cityInfoRepository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(_citiesRepository.GetAllCities());
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var cityToReturn = _citiesRepository.GetByCityId(id);
            if (cityToReturn == null)
            {
                return NotFound();
            }
            return Ok(cityToReturn);
        }
    }
}
