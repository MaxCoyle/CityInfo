using CityInfo.API.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CityInfo.API.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly string _connectionString;
        private readonly IUnitOfWork _unitOfWork;

        public CityRepository(string connectionString, Func<SqlConnection, IUnitOfWork> createUnitOfWork)
        {
            _connectionString = connectionString;
            _unitOfWork = createUnitOfWork(GetConnection());
        }

        private SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        private IEnumerable<CityDto> GetCities(QueryFilter queryFilter = null)
        {
            var citiesByCityName = new Dictionary<string, CityDto>();
                using (var connection = GetConnection())
                {
                    const string sql = "SELECT City.Id, City.Name, City.Description, " +
                                       "PointOfInterest.Id, PointOfInterest.Name, PointOfInterest.Description FROM City " +
                                       "LEFT JOIN PointOfInterest ON City.Id = PointOfInterest.CityId";

                    connection.Query<CityDto, PointOfInterestDto, CityDto>(
                        queryFilter != null ? $"{sql}  {queryFilter.Clause}" : sql,
                        (city, pointOfInterest) =>
                        {
                            if (!citiesByCityName.TryGetValue(city.Name, out var selectedCity))
                            {
                                citiesByCityName.Add(city.Name, city);
                            }
                            else
                            {
                                city = selectedCity;
                            }

                            city.PointsOfInterest.Add(pointOfInterest);

                            return city;

                        },
                        queryFilter?.Param);

                    return citiesByCityName.Values.ToList();
                }
            
        }
        public IEnumerable<CityDto> GetAllCities()
        {
            return GetCities();
        }
    }
}