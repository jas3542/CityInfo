using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.Entities;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.WindowsAzure.Storage;

namespace CityInfo.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository cityInfoRepository;
        
        //constructor for injection
        public CitiesController(ICityInfoRepository cityInfoRepositoryy)
        {
            cityInfoRepository = cityInfoRepositoryy;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            var cityEntities = cityInfoRepository.GetCities();
            // "Criteria - Jas"
            var results = Mapper.Map<IEnumerable<CityWIthoutPointsOfInterestDto>>(cityEntities);

            return Ok(results);
        }
        
        [HttpGet("{idd}")]
        public IActionResult GetCity(int idd, bool includePointsOfInterest = false)
        {
            var city = cityInfoRepository.GetCity(idd, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterestResult = Mapper.Map<CityWIthoutPointsOfInterestDto>(city);
            return Ok(cityWithoutPointsOfInterestResult);
            
        }
    }
}
