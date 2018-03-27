using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Entities;

namespace CityInfo.Services
{
    public interface ICityInfoRepository
    {
        bool CityExists(int cityIdd);

        IEnumerable<City> GetCities();

        City GetCity(int cityId, bool includePointsOfInterestt);

        IEnumerable<PointOfInterest> GetPointsOfInterestsForCity(int cityId);

        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);

        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        bool Save();
    }
}
