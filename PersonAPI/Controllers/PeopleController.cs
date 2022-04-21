using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonAPI.Models;
using PersonAPI.Services;

namespace PersonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PersonService _person;

        public PeopleController(PersonService service) =>
            _person = service;

        [HttpGet]
        public ActionResult<List<Person>> Get() =>
            _person.Get();

        [HttpGet("{id:length(24)}", Name = "GetPerson")]
        public ActionResult<Person> Get(string id)
        {
            var person = _person.Get(id);

            if (person == null)
                return NotFound();

            return person;
        }

        [HttpGet("name/{name}")]
        public ActionResult<Person> GetByName(string name)
        {
            var person = _person.GetByName(name);

            if (person == null)
                return NotFound();

            return person;
        }

        [HttpGet("status/{status}")]
        public ActionResult<List<Person>> GetByStatus(string status)
        {
            var person = _person.GetByStatus(status);

            if (person == null)
                return NotFound();

            return person;
        }

        [HttpPost]
        public IActionResult Create(Person person)
        {
            if (_person.Create(person) == null)
                return BadRequest();

            return CreatedAtRoute("GetPerson", new { id = person.Id.ToString() }, person);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Person personIn)
        {
            var person = _person.Get(id);

            if (person == null)
                return NotFound();

            if (_person.Update(id, personIn) == null)
                return BadRequest();

            return Ok();
        }

        [HttpPut("name/{name}")]
        public IActionResult UpdateByName(string name, Person personIn)
        {
            var person = _person.GetByName(name);

            if (person == null)
                return NotFound();

            if (_person.UpdateByName(name, personIn) == null)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var person = _person.Get(id);

            if (person == null)
                return NotFound();

            _person.Remove(id);
            return NoContent();
        }
    }
}
