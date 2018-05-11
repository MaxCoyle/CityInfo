using CityInfo.API.Models;
using System.Collections.Generic;

namespace CityInfo.API.Repository
{
    public interface ICityRepository
    {
        IEnumerable<CityDto> GetAllCities();
    }
}