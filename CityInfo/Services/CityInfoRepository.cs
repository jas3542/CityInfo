using System.Collections.Generic;
using System.Linq;
using CityInfo.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Services
{
    public class CityInfoRepository : ICityInfoRepository

    {
        private CityInfoContext context;
        //Constructor
        public CityInfoRepository(CityInfoContext contextt)
        {
            context = contextt;
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(pointOfInterest);
        }

        public bool CityExists(int cityIdd)
        {
            return context.Cities.Any(c => c.Id == cityIdd);
        }

        public IEnumerable<City> GetCities()
        {
            return context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterestt)
        {
            if (includePointsOfInterestt)
            {
                return context.Cities.Include(c => c.PointsOfInterest)
                    .SingleOrDefault(c => c.Id == cityId);
            }
            return context.Cities
                .SingleOrDefault(c => c.Id == cityId);

        }

        public PointOfInterest GetPointOfInterestForCity(int cityIdd, int pointsOfInterestIdd)
        {
            return context.PointsOfInterest
                .SingleOrDefault(p => p.City.Id == cityIdd && p.Id == pointsOfInterestIdd);
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestsForCity(int cityIdd)
        {
            return context.PointsOfInterest
                .Where(p => p.City.Id == cityIdd).ToList();
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterestt)
        {
            context.PointsOfInterest.Remove(pointOfInterestt);
        }

        //save changes to the DB method
        public bool Save()
        {
            return (context.SaveChanges() >= 0);
        }

    }
}
