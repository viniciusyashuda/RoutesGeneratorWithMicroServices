using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamAPI.Models;

namespace TeamAPI.Services
{
    public class PersonQueries
    {
        static readonly HttpClient client = new HttpClient();

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

        public static async Task<Person> GetPersonByName(string name)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44358/api/People/name/" + name);
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

        public static async void UpdatePersonStatus(string name, Person personIn)
        {
            try
            {
                var json = JsonConvert.SerializeObject(personIn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44358/api/People/name/" + name, content);
                response.EnsureSuccessStatusCode();

            }
            catch
            {
                throw;
            }
        }
    }
}
