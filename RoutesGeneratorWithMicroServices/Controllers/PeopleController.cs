using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoutesGeneratorWithMicroServices.Data;
using RoutesGeneratorWithMicroServices.Models;
using RoutesGeneratorWithMicroServices.Services;

namespace RoutesGeneratorWithMicroServices.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await PersonQueries.GetAllPeople());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var person = await PersonQueries.GetPersonById(id);
            if (person == null)
                return NotFound();

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
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var person = await PersonQueries.GetPersonById(id);
            if (person == null)
                return NotFound();

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
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    PersonQueries.UpdatePerson(id, person);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (PersonExists(person.Id) == null)
                        return NotFound();
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
                return NotFound();

            var person = await PersonQueries.GetPersonById(id);
            if (person == null)
                return NotFound();

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var person = await PersonQueries.GetPersonById(id);
            if (person == null)
                return NotFound();

            PersonQueries.DeletePerson(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<Person> PersonExists(string id)
        {
            return await PersonQueries.GetPersonById(id);
        }
    }
}
