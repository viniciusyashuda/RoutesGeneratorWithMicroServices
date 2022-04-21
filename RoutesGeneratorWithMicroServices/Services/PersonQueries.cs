using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RoutesGeneratorWithMicroServices.Models;

namespace RoutesGeneratorWithMicroServices.Services
{
    public class PersonQueries
    {
        static readonly HttpClient client = new HttpClient();

        public static async Task<List<Person>> GetAllPeople()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44358/api/People");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var people = JsonConvert.DeserializeObject<List<Person>>(responseBody);

                return people;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Person> GetPersonById(string id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44358/api/People/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var person = JsonConvert.DeserializeObject<Person>(responseBody);

                return person;
            }
            catch
            {
                return null;
            }
        }

        public static async void PostPerson(Person personIn)
        {
            try
            {
                var json = JsonConvert.SerializeObject(personIn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:44358/api/People/", content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public static async void UpdatePerson(string id, Person personIn)
        {
            try
            {
                var json = JsonConvert.SerializeObject(personIn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44358/api/People/" + id, content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public static async void DeletePerson(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync("https://localhost:44358/api/People/" + id);
            response.EnsureSuccessStatusCode();
        }
    }
}
