using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RoutesGeneratorWithMicroServices.Models;

namespace RoutesGeneratorWithMicroServices.Services
{
    public class CityQueries
    {
        static readonly HttpClient client = new HttpClient();

        public static async Task<List<City>> GetAllCities()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44320/api/Cities");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var city = JsonConvert.DeserializeObject<List<City>>(responseBody);

                return city;
            }
            catch
            {
                return null;
            }
        }

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

        public static async void PostCity(City cityIn)
        {
            try
            {
                var json = JsonConvert.SerializeObject(cityIn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:44320/api/Cities/", content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public static async void UpdateCity(string id, City cityIn)
        {
            try
            {
                var json = JsonConvert.SerializeObject(cityIn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44320/api/Cities/" + id, content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public static async void DeleteCity(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync("https://localhost:44320/api/Cities/" + id);
            response.EnsureSuccessStatusCode();
        }
    }
}
