using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace GenteFit.Models.Usuarios
{
    public class User
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        public ObjectId Id { get; set; }
        [Required(ErrorMessage = "Debe completar este campo.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Debe completar este campo.")]
        public string Pass { get; set; }

        public User() { }

        public User(String email, String pass)
        {
            Email = email;
            Pass = pass;
        }
    }
}
