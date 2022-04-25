using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RoutesGeneratorWithMicroServices.Models;

namespace RoutesGeneratorWithMicroServices.Services
{
    public class UserQueries
    {
        static readonly HttpClient client = new HttpClient();

        public static async Task<List<User>> GetAllUsers()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44386/api/Users");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var city = JsonConvert.DeserializeObject<List<User>>(responseBody);

                return city;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<User> GetUserById(string id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44386/api/Users/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var city = JsonConvert.DeserializeObject<User>(responseBody);

                return city;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<User> GetUserByLogin(string login)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44386/api/Users/login/" + login);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(responseBody);

                return user;
            }
            catch
            {
                return null;
            }
        }

        public static async void PostUser(User userIn)
        {
            try
            {
                var json = JsonConvert.SerializeObject(userIn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:44386/api/Users/", content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public static async void UpdateUser(string id, User userIn)
        {
            try
            {
                var json = JsonConvert.SerializeObject(userIn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44386/api/Users/" + id, content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public static async void DeleteUser(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync("https://localhost:44386/api/Users/" + id);
            response.EnsureSuccessStatusCode();
        }
    }
}
