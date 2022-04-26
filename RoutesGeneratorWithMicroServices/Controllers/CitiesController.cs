using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoutesGeneratorWithMicroServices.Models;
using RoutesGeneratorWithMicroServices.Services;

namespace RoutesGeneratorWithMicroServices.Controllers
{
    [Authorize]
    public class CitiesController : Controller
    {
        // GET: Cities
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
            return View(await CityQueries.GetAllCities());
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                TempData["error"] = "O Id é necessário para a ação!";
                return NotFound();
            }

            var city = await CityQueries.GetCityById(id);
            if (city == null)
            {
                TempData["error"] = "Cidade não encontrada!";
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,FederativeUnit")] City city)
        {
            if (ModelState.IsValid)
            {
                CityQueries.PostCity(city);
                TempData["success"] = "Cidade criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                TempData["error"] = "O Id é necessário para a ação!";
                return NotFound();
            }

            var city = await CityQueries.GetCityById(id);
            if (city == null)
            {
                TempData["error"] = "Cidade não encontrada!";
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Name,FederativeUnit")] City city)
        {
            if (id != city.Id)
            {
                TempData["error"] = "O Id passado não é válido!";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CityQueries.UpdateCity(id, city);
                    TempData["success"] = "Cidade editada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (CityExists(city.Id) == null)
                    {
                        TempData["error"] = "Cidade não existe!";
                        return NotFound();
                    }
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                TempData["error"] = "O Id é necessário para a ação!";
                return NotFound();
            }

            var city = await CityQueries.GetCityById(id);
            if (city == null)
            {
                TempData["error"] = "Cidade não encontrada!";
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var city = await CityQueries.GetCityById(id);
            if (city == null)
            {
                TempData["error"] = "Cidade não encontrada!";
                return NotFound();
            }

            CityQueries.DeleteCity(id);
            TempData["success"] = "Cidade excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<City> CityExists(string id)
        {
            return await CityQueries.GetCityById(id);
        }
    }
}
