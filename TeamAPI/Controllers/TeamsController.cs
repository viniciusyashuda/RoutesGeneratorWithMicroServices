using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamAPI.Models;
using TeamAPI.Services;

namespace TeamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {

        private readonly TeamService _team;

        public TeamsController(TeamService service) =>
            _team = service;

        [HttpGet]
        public ActionResult<List<Team>> Get() =>
            _team.Get();

        [HttpGet("{id:length(24)}", Name = "GetTeam")]
        public ActionResult<Team> Get(string id)
        {
            var team = _team.Get(id);

            if (team == null)
                return NotFound();

            return team;
        }

        [HttpGet("name/{name}")]
        public ActionResult<Team> GetByName(string name)
        {
            var team = _team.GetByName(name);

            if (team == null)
                return NotFound();

            return team;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Team team)
        {
            if (await _team.Create(team) == null)
                return BadRequest();

            return CreatedAtRoute("GetTeam", new { id = team.Id.ToString() }, team);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Team teamIn)
        {
            var team = _team.Get(id);

            if (team == null)
                return NotFound();

            if (await _team.Update(id, teamIn) == null)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var team = _team.Get(id);

            if (team == null)
                return NotFound();

            _team.Remove(id);
            return NoContent();
        }

    }
}
