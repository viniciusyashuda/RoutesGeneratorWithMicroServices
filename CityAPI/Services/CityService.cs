using System.Collections.Generic;
using CityAPI.Config;
using CityAPI.Models;
using MongoDB.Driver;

namespace CityAPI.Services
{
    public class CityService
    {
        private readonly IMongoCollection<City> _city;

        public CityService(ICityDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _city = database.GetCollection<City>(settings.CityCollectionName);
        }

        public List<City> Get() =>
            _city.Find(city => true).ToList();

        public City Get(string id) =>
            _city.Find(city => city.Id == id).FirstOrDefault();
        public City GetByNameAndFederativeUnit(string name, string federative_unit) =>
            _city.Find(city => city.Name == name && city.FederativeUnit == federative_unit).FirstOrDefault();

        public City Create(City city)
        {
            if (GetByNameAndFederativeUnit(city.Name, city.FederativeUnit) != null)
                return null;

            _city.InsertOne(city);
            return city;
        }

        public City Update(string id, City cityIn)
        {
            var check = GetByNameAndFederativeUnit(cityIn.Name, cityIn.FederativeUnit);
            if (check != null && check.Id != cityIn.Id)
                return null;

            _city.ReplaceOne(city => city.Id == id, cityIn);
            return cityIn;
        }

        public void Remove(City cityIn) =>
            _city.DeleteOne(city => city.Id == cityIn.Id);

        public void Remove(string id) =>
            _city.DeleteOne(city => city.Id == id);
    }
}
