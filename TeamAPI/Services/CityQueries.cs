using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamAPI.Models;

namespace TeamAPI.Services
{
    public class CityQueries
    {
        static readonly HttpClient client = new HttpClient();

        public static async Task<City> GetCityById(string id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44320/api/Cities/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var city = JsonConvert.DeserializeObject<City>(responseBody);

                return city;
            }
            catch
            {
                return null;   
            }
        }

        public static async Task<City> GetCityByNameAndFederativeUnit(string name, string federativeUnit)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44320/api/Cities/name/" + name + "/federativeUnit/" + federativeUnit);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var city = JsonConvert.DeserializeObject<City>(responseBody);

                return city;
            }
            catch
            {
                return null;
            }
        }
    }
}
