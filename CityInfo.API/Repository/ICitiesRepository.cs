using CityInfo.API.Models;
using System.Collections.Generic;

namespace CityInfo.API.Repository
{
    public interface ICitiesRepository
    {
        IEnumerable<CityDto> GetAllCities();

        CityDto GetByCityId(int cityId);
    }
}