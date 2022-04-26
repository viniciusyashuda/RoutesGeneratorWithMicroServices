using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace RoutesGeneratorWithMicroServices.Models
{
    public class City
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [JsonProperty("name")]
        [Display(Name ="Nome")]
        public string Name { get; set; }
        [JsonProperty("federativeUnit")]
        [Display(Name = "Estado")]
        public string FederativeUnit { get; set; }
    }
}
