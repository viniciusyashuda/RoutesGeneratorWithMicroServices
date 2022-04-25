using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RoutesGeneratorWithMicroServices.Models;
using RoutesGeneratorWithMicroServices.Services;

namespace RoutesGeneratorWithMicroServices.Controllers
{
   // [Authorize]
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

        public IActionResult ServiceChoice()
        {
            var selectedHeaders = Request.Form["Column"].ToList();
            List<string> services = new();

            foreach (var header in selectedHeaders)
                if (header == "SERVIÇO" || header == "SERVICO")
                    services = ReadFile.ReadColumn(header, _appEnvironment.WebRootPath);

            ViewBag.Services = services.Distinct().ToList();
            ViewBag.SelectedHeaders = selectedHeaders;
            return View();
        }

        public IActionResult CityChoice(string service)
        {
            List<string> cities = new();

            var selectedHeaders = Request.Form["headers"].ToList();

            foreach (var header in selectedHeaders)
                if (header == "CIDADE")
                    cities = ReadFile.ReadColumn(header, _appEnvironment.WebRootPath);

            ViewBag.Cities = cities.Distinct().ToList();
            ViewBag.Service = service;
            ViewBag.Headers = selectedHeaders;
            return View();
        }

        public async Task<IActionResult> TeamsChoice(string city)
        {
            var selectedHeaders = Request.Form["headers"].ToList();
            var servicerequest = Request.Form["service"].ToString();

            var teams = await TeamQueries.GetAllTeams();
            List<Team> teamsInCity = new();
            var service = servicerequest.Replace(",", "");

            foreach (var team in teams)
                if (team.City.Name == city)
                    teamsInCity.Add(team);

            ViewBag.ServiceToWrite = service;
            ViewBag.TeamsToWrite = teamsInCity;
            ViewBag.HeadersToWrite = selectedHeaders;
            ViewBag.CityToWrite = city;
            return View();
        }

        public IActionResult GenerateDocx()
        {
            var selectedHeaders = Request.Form["headers"].ToList();
            var servicerequest = Request.Form["service"].ToString();
            var cityrequest = Request.Form["city"].ToString();
            var teams = Request.Form["team"].ToList();
            
            var service = servicerequest.Replace(",", "");
            var city = cityrequest.Replace(",", "");

            new WriteFile().WriteDocxFile(selectedHeaders, teams, service, city, _appEnvironment.WebRootPath);
            return RedirectToAction(nameof(Index));
        }

    }
}
