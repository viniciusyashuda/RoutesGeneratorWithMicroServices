﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoutesGeneratorWithMicroServices.Models;
using RoutesGeneratorWithMicroServices.Services;

namespace RoutesGeneratorWithMicroServices.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        // GET: Teams
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
            return View(await TeamQueries.GetAllTeams());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                TempData["error"] = "Time não encotrado!";
                return NotFound();
            }

            var team = await TeamQueries.GetTeamById(id);
            if (team == null)
            {
                TempData["error"] = "Time não encontrado!";
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public async Task<IActionResult> Create()
        {
            var registeredPeople = await PersonQueries.GetAllPeople();
            var teams = await TeamQueries.GetAllTeams();
            var peopleWithTeam = new List<Person>();
            var peopleWithoutTeam = new List<Person>();

            foreach (var team in teams)
                foreach (var member in team.Members)
                    peopleWithTeam.Add(member);

            foreach (var registeredPerson in registeredPeople)
                if (peopleWithTeam.Find(x => x.Id.Equals(registeredPerson.Id)) == null)
                    peopleWithoutTeam.Add(registeredPerson);

            ViewBag.People = peopleWithoutTeam;
            ViewBag.Cities = await CityQueries.GetAllCities();
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Team team)
        {
            string cityId = Request.Form["City"].ToString();
            team.City = await CityQueries.GetCityById(cityId);

            var membersId = Request.Form["Members"].ToList();
            team.Members = new List<Person>();

            foreach (var memberId in membersId)
            {
                var member = await PersonQueries.GetPersonById(memberId);
                team.Members.Add(member);
            }

            if (ModelState.IsValid)
            {
                TeamQueries.PostTeam(team);
                TempData["success"] = "Time criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                TempData["error"] = "O Id é necessário para a ação!";
                return NotFound();
            }

            var team = await TeamQueries.GetTeamById(id);
            if (team == null)
            {
                TempData["error"] = "Time não encotrado!";
                return NotFound();
            }

            var registeredPeople = await PersonQueries.GetAllPeople();
            var teams = await TeamQueries.GetAllTeams();
            var peopleWithTeam = new List<Person>();
            var peopleWithoutTeam = new List<Person>();

            foreach (var teamIn in teams)
                foreach (var member in teamIn.Members)
                    peopleWithTeam.Add(member);

            foreach (var registeredPerson in registeredPeople)
                if (peopleWithTeam.Find(x => x.Id.Equals(registeredPerson.Id)) == null)
                    peopleWithoutTeam.Add(registeredPerson);

            ViewBag.TeamMembers = team.Members;
            ViewBag.People = peopleWithoutTeam;
            ViewBag.Cities = await CityQueries.GetAllCities();
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Team team)
        {
            if (id != team.Id)
            {
                TempData["error"] = "O Id passado não é valido!";
                return NotFound();
            }

            var team_name = team.Name;
            team = await TeamQueries.GetTeamById(id);
            team.Name = team_name;

            string cityId = Request.Form["City"].ToString();
            team.City = team.City = await CityQueries.GetCityById(cityId);

            var notTeamMembersId = Request.Form["NotTeamMembers"].ToList();
            var teamMembersId = Request.Form["TeamMembers"].ToList();

            if ((teamMembersId != null || teamMembersId.Any()) && (team.Members.Count > teamMembersId.Count))
                foreach (var memberId in teamMembersId)
                {
                    Person member = await PersonQueries.GetPersonById(memberId);
                    team.Members.Remove(team.Members.Find(memberToRemove => memberToRemove.Id == memberId));
                    member.Status = "No team";
                    PersonQueries.UpdatePerson(memberId, member);
                }

            foreach (var notMemberId in notTeamMembersId)
                team.Members.Add(await PersonQueries.GetPersonById(notMemberId));

            if (ModelState.IsValid)
            {
                try
                {
                    TeamQueries.UpdateTeam(id, team);
                    TempData["success"] = "Time editado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (TeamExists(team.Id) == null)
                    {
                        TempData["error"] = "Time não existe!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                TempData["error"] = "O Id é necessario para a ação!";
                return NotFound();
            }

            var team = await TeamQueries.GetTeamById(id);
            if (team == null)
            {
                TempData["error"] = "Time não encontrado!";
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var team = await TeamQueries.GetTeamById(id);
            if (team == null)
            {
                TempData["error"] = "Time não encontrado!";
                return NotFound();
            }

            TeamQueries.DeleteTeam(id);
            TempData["success"] = "Time excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<Team> TeamExists(string id)
        {
            return await TeamQueries.GetTeamById(id);
        }
    }
}
