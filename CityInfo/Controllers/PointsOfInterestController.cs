using System;
using System.Collections.Generic;
using AutoMapper;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.Controllers
{
    //points of interest without a city doesn't make sense so it has to be cities/{id}/pointsOfView
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> logger;
        private IMailService mailService;
        private ICityInfoRepository cityInfoRepository;

        // dependency injection using constructor
        public PointsOfInterestController(ILogger<PointsOfInterestController> loggerr, IMailService mailServicee, ICityInfoRepository cityInfoRepositoryy)
        {
            logger = loggerr;
            mailService = mailServicee;
            cityInfoRepository = cityInfoRepositoryy;

        }

        [HttpGet("{cityIdd}/pointsOfInterest")]
        public IActionResult GetPointOfInterest(int cityIdd)
        {
            try
            {
                if (!cityInfoRepository.CityExists(cityIdd))
                {
                    logger.LogInformation($"City with id {cityIdd} wasn't found when accessing points of interest.");
                    return NotFound();
                }
                
                var pointsOfInterestForCity = cityInfoRepository.GetPointsOfInterestsForCity(cityIdd);
                var pointsOfInterestForCityResults = Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);

                return Ok(pointsOfInterestForCityResults);
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Exception while getting points of interest for city with id {cityIdd}.",ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
        
        [HttpGet("{valueCityIdd}/pointsOfInterest/{valuePointIdd}", Name = "GetPointOfInterest")]
        public IActionResult GetPointsOfInterest(int valueCityIdd, int valuePointIdd)
        {
            if (!cityInfoRepository.CityExists(valueCityIdd))
            {
                return NotFound();
            }

            var pointOfInterest = cityInfoRepository.GetPointOfInterestForCity(valueCityIdd, valuePointIdd);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestResult = Mapper.Map<PointOfInterestDto>(pointOfInterest);
            return Ok(pointOfInterestResult);

        }

        [HttpPost("{cityIdd}/pointsOfInterest")]
        public IActionResult CreatePointOfInterest(int cityIdd,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            //ERRORS Control
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                // Second option to show a custom error message.
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }

            // To check if the post data is valid. for example : Instead of POI name we are not receiving cars Models.
            if (!ModelState.IsValid)
            {
                //Passing the modelState to show a custom error msg which we have hardcoded in the DTO class. It's Optional.
                return BadRequest(ModelState);
            }
            
            if (!cityInfoRepository.CityExists(cityIdd))
            {
                return NotFound();
            }
            
            var finalPointOfInterest = Mapper.Map<Entities.PointOfInterest>(pointOfInterest);
            
            cityInfoRepository.AddPointOfInterestForCity(cityIdd, finalPointOfInterest);

            if (!cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling you request");
            }

            // retriving the data from DB to DTO to show it to the user:
            var createdPointOfInterestToReturn = Mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            // GetPointOfInterest is used to get the pointOfInterest. Take a look to the above method, the attribute names.
            return CreatedAtRoute("GetPointOfInterest",
                new {valueCityIdd = cityIdd, valuePointIdd = createdPointOfInterestToReturn.Id}, createdPointOfInterestToReturn);
        }

        [HttpPut("{cityIDD}/pointsOfInterest/{pointIdd}")]
        public IActionResult UpdatePointOfInterest(int cityIdd, int pointIdd,
            [FromBody] PointOfInterestForUpdateDto pointOfInterestt)
        {
            if (pointOfInterestt == null)
            {
                return BadRequest();
            }

            //ERRORS Control
            if (pointOfInterestt.Description == pointOfInterestt.Name)
            {
                // Second option to show a custom error message.
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }

            // To check if the post data is valid. for example : Instead of POI name we are not receiving cars Models.
            if (!ModelState.IsValid)
            {
                //Passing the modelState to show a custom error msg which we have hardcoded in the DTO class. It's Optional.
                return BadRequest(ModelState);
            }

            if (!cityInfoRepository.CityExists(cityIdd))
            {
                return NotFound();
            }

            var pointOfInterestEntity = cityInfoRepository.GetPointOfInterestForCity(cityIdd,pointIdd);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(pointOfInterestt, pointOfInterestEntity);

            if (!cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling you request");
            }

            // nothing to return for put methods
            return NoContent();
        }

        [HttpPatch("{cityIDD}/pointsOfInterest/{pointIdd}")]
        public IActionResult PatiallyUpdatePointOfInterest(int cityIdd, int pointIdd,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDocc)
        {
            if (patchDocc == null)
            {
                return BadRequest();
            }

            if (!cityInfoRepository.CityExists(cityIdd))
            {
                return NotFound();
            }

            var pointOfInterestEntity = cityInfoRepository.GetPointOfInterestForCity(cityIdd, pointIdd);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            // we apply the json patch document to the object that we want to update. ModelState is passed to save any error occured with the patching.
            patchDocc.ApplyTo(pointOfInterestToPatch, ModelState);

            //ERRORS Control
            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                // Second option to show a custom error message.
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }
            
            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                //Passing the modelState to show a custom error msg which we have hardcoded in the DTO class. It's Optional.
                return BadRequest(ModelState);
            }
            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            if (!cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling you request");
            }

            return NoContent();
        }

        [HttpDelete("{cityIdd}/pointsOfInterest/{pointIdd}")]
        public IActionResult DeletePointOfInterest(int cityIdd, int pointIdd)
        {
            if (!cityInfoRepository.CityExists(cityIdd))
            {
                return NotFound();
            }

            var pointOfInterestFromEntity = cityInfoRepository.GetPointOfInterestForCity(cityIdd, pointIdd);
            if (pointOfInterestFromEntity == null)
            {
                return NotFound();
            }
            
            cityInfoRepository.DeletePointOfInterest(pointOfInterestFromEntity);

            if (!cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling you request");
            }
            
            mailService.Send("Point of interest deleted", $"Point of interest {pointOfInterestFromEntity.Name} with id {pointOfInterestFromEntity.Id} was deleted");

            return NoContent();
        }
        
    }
}