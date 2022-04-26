using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace RoutesGeneratorWithMicroServices.Models
{
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [JsonProperty("name")]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [JsonProperty("city")]
        [Display(Name = "Cidade")]
        public City City { get; set; }
        [JsonProperty("members")]
        [Display(Name = "Membros")]
        public List<Person> Members { get; set; }
    }
}
