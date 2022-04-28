using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoutesGeneratorWithMicroServices.Models;
using RoutesGeneratorWithMicroServices.Services;

namespace RoutesGeneratorWithMicroServices.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public IActionResult Index()
        {
            string user;
            bool authenticate = false;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = HttpContext.User.Identity.Name;
                authenticate = true;

                if (HttpContext.User.IsInRole("Admin"))
                    ViewBag.Role = "Admin";
                else
                    ViewBag.Role = "User";
            }
            else
            {
                user = "Not logged!";
                ViewBag.Role = "";
            }

            ViewBag.User = user;
            ViewBag.Authenticate = authenticate;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            //string login = Request.Form["login"];
            //string password = Request.Form["password"];

            var userFound = await UserQueries.GetUserByLogin(user.Login);

            if (userFound != null && userFound.Password == user.Password)
            {
                List<Claim> userClaim = new()
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim("Role", user.Login),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(userClaim, "User");
                var mainUser = new ClaimsPrincipal(new[] { identity });

                await HttpContext.SignInAsync(mainUser);

                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var user = await UserQueries.GetUserById(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            var user_aux = await UserQueries.GetUserByLogin(HttpContext.User.Identity.Name);
            if (user_aux == null || user_aux.Role != "Admin")
            {
                TempData["error"] = "Usuário não permitido!";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Login,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                UserQueries.PostUser(user);
                TempData["success"] = "Usuário criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var user = await UserQueries.GetUserById(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Name,Login,Password")] User user)
        {
            if (id != user.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    UserQueries.UpdateUser(id, user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (UserExists(user.Id) == null)
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var user = await UserQueries.GetUserById(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await UserQueries.GetUserById(id);
            if (user == null)
                return NotFound();

            UserQueries.DeleteUser(id);

            return RedirectToAction(nameof(Index));
        }

        private static async Task<User> UserExists(string id)
        {
            return await UserQueries.GetUserById(id);
        }
    }
}
