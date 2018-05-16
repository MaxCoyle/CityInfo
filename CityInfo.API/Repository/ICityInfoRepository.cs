using CityInfo.API.Models;
using System.Collections.Generic;

namespace CityInfo.API.Repository
{
    public interface ICityInfoRepository
    {
        IEnumerable<CityDto> GetAllCities();

        CityDto GetByCityId(int cityId);
    }
}