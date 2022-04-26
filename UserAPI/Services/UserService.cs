using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using UserAPI.Config;
using UserAPI.Model;

namespace UserAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _user;

        public UserService(IUserDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>(settings.UserCollectionName);
        }

        public List<User> Get() =>
            _user.Find(user => true).ToList();

        public User Get(string id) =>
            _user.Find(user => user.Id == id).FirstOrDefault();

        public User GetByLogin(string login) =>
            _user.Find(user => user.Login == login).FirstOrDefault();

        public async Task<User> Create(User user)
        {
            if (GetByLogin(user.Login) != null)
                return null;

            _user.InsertOne(user);
            return user;
        }

        public async Task<User> Update(string id, User userIn)
        {
            _user.ReplaceOne(user => user.Id == id, userIn);
            return userIn;
        }

        public void Remove(User userIn) =>
            _user.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _user.DeleteOne(user => user.Id == id);
    }
}
