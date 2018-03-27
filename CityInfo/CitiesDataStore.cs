using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Models;

namespace CityInfo
{
    public class CitiesDataStore
    {
        // returns the instance of the DataStore ( a lazy singleton). 
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "They have a big park",

                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "It's Urban park",
                        },
                    
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Empire State",
                            Description = "A very big building",
                        },
                    },
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Barcelona",
                    Description = "They have very tasty Food",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Gaudi Park",
                            Description = "Also a Urban Park. Gaudi style",
                        },

                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Montjuic Castle",
                            Description = "It's a castle on the Montjuic montain",
                        },
                    },
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Dublin",
                    Description = "Techy city",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Stephan green",
                            Description = "A very green park. It's a square",
                        },

                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Phoenix Park",
                            Description = "The largest park in europe",
                        },
                    },
                }
            };

        }
    }
}
