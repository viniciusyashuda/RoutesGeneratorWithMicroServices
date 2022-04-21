using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TeamAPI.Models
{
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("city")]
        public City City { get; set; }
        [JsonProperty("members")]
        public List<Person> Members { get; set; }
    }
}
