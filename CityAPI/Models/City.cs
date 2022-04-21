using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CityAPI.Models
{
    public class City
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("federativeUnit")]
        public string FederativeUnit { get; set; }
    }
}
