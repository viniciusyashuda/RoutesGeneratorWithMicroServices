using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RoutesGeneratorWithMicroServices.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Usuário")]
        public string Login { get; set; }
        [Display(Name = "Senha")]
        public string Password { get; set; }
        public string Role { get; set; } = "User";
    }
}
