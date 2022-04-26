using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoutesGeneratorWithMicroServices.Models;
using RoutesGeneratorWithMicroServices.Services;

namespace RoutesGeneratorWithMicroServices.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        // GET: People
        public async Task<IActionResult> Index()
        {
            string user = "Anonymous";
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
                authenticate = false;
                ViewBag.Role = "";
            }

            ViewBag.User = user;
            ViewBag.Authenticate = authenticate;
            return View(await PersonQueries.GetAllPeople());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                TempData["error"] = "O Id é necessario para a ação!";
                return NotFound();
            }

            var person = await PersonQueries.GetPersonById(id);
            if (person == null)
            {
                TempData["error"] = "Pessoa não encontrada!";
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Status")] Person person)
        {
            if (ModelState.IsValid)
            {
                PersonQueries.PostPerson(person);
                TempData["success"] = "Pessoa criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                TempData["error"] = "O Id é necessário para a ação!";
                return NotFound();
            }

            var person = await PersonQueries.GetPersonById(id);
            if (person == null)
            {
                TempData["error"] = "Pessoa não encontrada!";
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Name,Status")] Person person)
        {
            if (id != person.Id)
            {
                TempData["error"] = "O Id passado não é válido!";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PersonQueries.UpdatePerson(id, person);
                    TempData["success"] = "Pessoa editada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (PersonExists(person.Id) == null)
                    {
                        TempData["error"] = "Pessoa não existe!";
                        return NotFound();
                    }
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                TempData["error"] = "O Id é necessario para a ação!";
                return NotFound();
            }

            var person = await PersonQueries.GetPersonById(id);
            if (person == null)
            {
                TempData["error"] = "Pessoa não encontrada!";
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var person = await PersonQueries.GetPersonById(id);
            if (person == null)
            {
                TempData["error"] = "Pessoa não encontrada!";
                return NotFound();
            }

            PersonQueries.DeletePerson(id);
            TempData["success"] = "Pessoa excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<Person> PersonExists(string id)
        {
            return await PersonQueries.GetPersonById(id);
        }
    }
}
