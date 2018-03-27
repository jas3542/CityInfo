using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Entities;

namespace CityInfo
{
    public static class CityInfoContextExtensions
    {
        // contexx extends CityInfoContext. We say that using this.
        public static void EnsureSeedDataForContext(this CityInfoContext contextt)
        {
            if (contextt.Cities.Any())
            {
                return;
            }

            // init seed data for DB
            var Cities = new List<City>()
            {
                new City()
                {
                    Name = "New York City",
                    Description = "They have a big park",

                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = "It's Urban park",
                        },

                        new PointOfInterest()
                        {
                            Name = "Empire State",
                            Description = "A very big building",
                        },
                    },
                },
                new City()
                {
                    Name = "Barcelona",
                    Description = "They have very tasty Food",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Gaudi Park",
                            Description = "Also a Urban Park. Gaudi style",
                        },

                        new PointOfInterest()
                        {
                            Name = "Montjuic Castle",
                            Description = "It's a castle on the Montjuic montain",
                        },
                    },
                },
                new City()
                {
                    Name = "Dublin",
                    Description = "Techy city",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Stephan green",
                            Description = "A very green park. It's a square",
                        },

                        new PointOfInterest()
                        {
                            Name = "Phoenix Park",
                            Description = "The largest park in europe",
                        },
                    },
                }
            };
            // insert into context
            contextt.Cities.AddRange(Cities);
            contextt.SaveChanges();
        }
    }
}
