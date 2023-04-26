using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace GenteFit.Models.Usuarios
{
    public class User
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Debe completar este campo.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Debe completar este campo.")]
        public string Pass { get; set; } = null!;
        public string Tipo { get; set; } = null!;

        public User() { }

        public User(String email, String pass)
        {
            Email = email;
            Pass = pass;
        }
    }
}
