using System.Collections.Generic;
using MongoDB.Driver;
using PersonAPI.Config;
using PersonAPI.Models;

namespace PersonAPI.Services
{
    public class PersonService
    {
        private readonly IMongoCollection<Person> _person;

        public PersonService(IPersonDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _person = database.GetCollection<Person>(settings.PersonCollectionName);
        }

        public List<Person> Get() =>
            _person.Find(city => true).ToList();

        public Person Get(string id) =>
            _person.Find(person => person.Id == id).FirstOrDefault();

        public Person GetByName(string name) =>
            _person.Find(person => person.Name == name).FirstOrDefault();

        public Person Create(Person person)
        {
            if (GetByName(person.Name) != null)
                return null;

            person.Status = "Sem time";
            _person.InsertOne(person);
            return person;
        }

        public Person Update(string id, Person personIn)
        {
            var personFound = GetByName(personIn.Name);
            if (personFound != null && personFound.Id != personIn.Id)
                return null;

            _person.ReplaceOne(person => person.Id == id, personIn);
            return personIn;
        }
        public Person UpdateByName(string name, Person personIn)
        {
            var personFound = GetByName(personIn.Name);
            if (personFound != null && personFound.Id != personIn.Id)
                return null;

            _person.ReplaceOne(person => person.Name == name, personIn);
            return personIn;
        }

        public void Remove(Person personIn) =>
            _person.DeleteOne(person => person.Id == personIn.Id);

        public void Remove(string id)
        {
            _person.DeleteOne(person => person.Id == id);
        }
    }
}
