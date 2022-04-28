using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using TeamAPI.Config;
using TeamAPI.Models;

namespace TeamAPI.Services
{
    public class TeamService
    {
        private readonly IMongoCollection<Team> _team;

        public TeamService(ITeamDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _team = database.GetCollection<Team>(settings.TeamCollectionName);
        }

        public List<Team> Get() =>
            _team.Find(team => true).ToList();

        public Team Get(string id) =>
            _team.Find(team => team.Id == id).FirstOrDefault();

        public Team GetByName(string name) =>
            _team.Find(team => team.Name == name).FirstOrDefault();

        public async Task<Team> Create(Team team)
        {
            if (GetByName(team.Name) != null)
                return null;

            var city = await CityQueries.GetCityByNameAndFederativeUnit(team.City.Name, team.City.FederativeUnit);
            if (city == null)
                return null;

            if (team.Members == null)
                return null;

            List<Person> temp_memberslist = new();
            foreach (var member in team.Members)
            {
                var membertemp = await PersonQueries.GetPersonByName(member.Name);

                if (membertemp == null)
                    return null;
                else if (membertemp.Status == "Em um time")
                    return null;
                else
                {
                    membertemp.Status = "Em um time";
                    PersonQueries.UpdatePersonStatus(membertemp.Name, membertemp);
                    temp_memberslist.Add(membertemp);
                }

            }

            team.Members = temp_memberslist;
            team.City = city;
            _team.InsertOne(team);
            return team;
        }

        public async Task<Team> Update(string id, Team teamIn)
        {
            var city = await CityQueries.GetCityByNameAndFederativeUnit(teamIn.City.Name, teamIn.City.FederativeUnit);
            if (city == null)
                return null;

            if (teamIn.Members == null)
                return null;

            List<Person> temp_memberslist = new();
            foreach (var member in teamIn.Members)
            {
                var membertemp = await PersonQueries.GetPersonByName(member.Name);

                if (membertemp == null)
                    return null;
                else
                {
                    membertemp.Status = "Em um time";
                    PersonQueries.UpdatePersonStatus(membertemp.Name, membertemp);
                    temp_memberslist.Add(membertemp);
                }

            }

            if (temp_memberslist.Count == 0)
                return null;

            teamIn.Members = temp_memberslist;
            teamIn.City = city;
            _team.ReplaceOne(team => team.Id == id, teamIn);
            return teamIn;
        }

        public void Remove(Team teamIn) =>
            _team.DeleteOne(team => team.Id == teamIn.Id);

        public void Remove(string id)
        {
            var team = Get(id);

            foreach (var member in team.Members)
            {
                member.Status = "Em um time";
                PersonQueries.UpdatePersonStatus(member.Name, member);
            }
            _team.DeleteOne(team => team.Id == id);
        }
    }
}
