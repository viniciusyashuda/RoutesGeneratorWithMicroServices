using System.Collections.Generic;
using CityAPI.Models;
using CityAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CityService _city;

        public CitiesController(CityService service) =>
            _city = service;

        [HttpGet]
        public ActionResult<List<City>> Get() =>
            _city.Get();

        [HttpGet("{id:length(24)}", Name = "GetCity")]
        public ActionResult<City> Get(string id)
        {
            var city = _city.Get(id);

            if (city == null)
                return NotFound();

            return city;
        }

        [HttpGet("name/{name}/federativeUnit/{federativeUnit}")]
        public ActionResult<City> GetByNameAndFederativeUnit(string name, string federativeUnit)
        {
            var city = _city.GetByNameAndFederativeUnit(name, federativeUnit);

            if (city == null)
                return NotFound();

            return city;
        }

        [HttpPost]
        public IActionResult Create(City city)
        {
            if (_city.Create(city) == null)
                return BadRequest();

            return CreatedAtRoute("GetCity", new { id = city.Id.ToString() }, city);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, City cityIn)
        {
            var city = _city.Get(id);

            if (city == null)
                return NotFound();
            if (_city.Update(id, cityIn) == null)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var city = _city.Get(id);

            if (city == null)
                return NotFound();

            _city.Remove(id);
            return NoContent();
        }
    }
}
