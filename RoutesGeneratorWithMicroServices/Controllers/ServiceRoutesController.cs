using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RoutesGeneratorWithMicroServices.Models;
using RoutesGeneratorWithMicroServices.Services;

namespace RoutesGeneratorWithMicroServices.Controllers
{
    public class ServiceRoutesController : Controller
    {
        IWebHostEnvironment _appEnvironment;

        public ServiceRoutesController(IWebHostEnvironment environment)
        {
            _appEnvironment = environment;
        }
        public IActionResult Index()
        {
            ViewBag.Headers = ReadFile.ReadHeader(_appEnvironment.WebRootPath); 
            return View();
        }

        //public IActionResult ReadFile()
        //{
        //    var header = Request.Form["Column"].ToList();
        //    var data = new ReadFile().ReadExcelFile(header, _appEnvironment.WebRootPath);
        //    return RedirectToAction(nameof());
        //}

        public IActionResult ServiceChoice()
        {
            var headers = Request.Form["Column"].ToList();
            List<string> services = new();

            foreach (var header in headers)
                if (header == "SERVIÇO" || header == "SERVICO")
                    services = ReadFile.ReadColumn(header, _appEnvironment.WebRootPath);

            ViewBag.Services = services.Distinct().ToList();
            ViewBag.SelectedHeaders = headers;
            return View();
        }

        public IActionResult CityChoice(string service, List<string> headers)
        {
            List<string> cities = new();

            foreach (var header in headers)
                if (header == "CIDADE")
                    cities = ReadFile.ReadColumn(header, _appEnvironment.WebRootPath);

            ViewBag.Cities = cities.Distinct().ToList();
            ViewBag.Service = service;
            return View();
        }

        public async Task<IActionResult> TeamsChoice(string city)
        {
            var teams = await TeamQueries.GetAllTeams();
            List<Team> teamsInCity = new();

            foreach (var team in teams)
                if (team.City.Name == city)
                    teamsInCity.Add(team);

            ViewBag.Teams = teamsInCity;
            return View();
        }

    }
}
