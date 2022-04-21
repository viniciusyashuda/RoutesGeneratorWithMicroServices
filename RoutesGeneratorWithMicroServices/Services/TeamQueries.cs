using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RoutesGeneratorWithMicroServices.Models;

namespace RoutesGeneratorWithMicroServices.Services
{
    public class TeamQueries
    {
        static readonly HttpClient client = new HttpClient();

        public static async Task<List<Team>> GetAllTeams()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44330/api/Teams");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var teams = JsonConvert.DeserializeObject<List<Team>>(responseBody);

                return teams;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Team> GetTeamById(string id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44330/api/Teams/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var team = JsonConvert.DeserializeObject<Team>(responseBody);

                return team;
            }
            catch
            {
                return null;
            }
        }

        public static async void PostTeam(Team teamIn)
        {
            try
            {
                var json = JsonConvert.SerializeObject(teamIn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:44330/api/Teams/", content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }
        public static async void UpdateTeam(string id, Team teamIn)
        {
            try
            {
                var json = JsonConvert.SerializeObject(teamIn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44330/api/Teams/" + id, content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public static async void DeleteTeam(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync("https://localhost:44330/api/Teams/" + id);
            response.EnsureSuccessStatusCode();
        }
    }
}
