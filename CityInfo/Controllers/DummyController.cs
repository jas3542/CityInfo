﻿using CityInfo.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Controllers
{
    public class DummyController : Controller
    {
        private CityInfoContext ctx;

        public DummyController(CityInfoContext ctxx)
        {
            ctx = ctxx;
        }

        [HttpGet]
        [Route("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
