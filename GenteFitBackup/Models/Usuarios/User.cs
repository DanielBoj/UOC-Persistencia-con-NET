using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GenteFit.Models.Usuarios
{
    public class User
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; } = null!;
        [BsonElement("Pass")]
        public string Pass { get; set; } = null!;
        [BsonElement("Tipo"), BsonDefaultValue(null), BsonIgnoreIfNull, BsonIgnoreIfDefault]
        public string Tipo { get; set; } = null!;

        public User() { }

        public User(String email, String pass)
        {
            Email = email;
            Pass = pass;
        }
    }
}
