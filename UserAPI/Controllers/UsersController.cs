using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Model;
using UserAPI.Services;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _user;

        public UsersController(UserService service) =>
            _user = service;

        [HttpGet]
        public ActionResult<List<User>> Get() =>
            _user.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var user = _user.Get(id);

            if (user == null)
                return NotFound();

            return user;
        }

        [HttpGet("login/{login}")]
        public ActionResult<User> GetByLogin(string login)
        {
            var user = _user.GetByLogin(login);

            if (user == null)
                return NotFound();

            return user;
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (await _user.Create(user) == null)
                return BadRequest();

            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User userIn)
        {
            var user = _user.Get(id);

            if (user == null)
                return NotFound();

            if (await _user.Update(id, userIn) == null)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var user = _user.Get(id);

            if (user == null)
                return NotFound();

            _user.Remove(id);
            return NoContent();
        }

    }
}

