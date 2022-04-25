﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RoutesGeneratorWithMicroServices.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User";
    }
}
