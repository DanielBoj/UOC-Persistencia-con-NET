using GenteFit.Models.Prototypes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GenteFit.Models
{
    public class Centro
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Nombre")]
        public string Nombre { get; set; } = null!;
        [BsonElement("Direccion"), BsonDefaultValue(null), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public Direccion Direccion { get; set; } = null!;
        [BsonElement("Descripcion")]
        public string Descripcion { get; set; } = null!;
        [BsonElement("Telefono")]
        public string Telefono { get; set; } = null!;
        [BsonElement("Email")]
        public string Email { get; set; } = null!;

        public Centro() {
            Direccion = new();
        }

        public Centro(string nombre, Direccion direccion, string descripcion, string telefono, string email)
        {
            Nombre = nombre;
            Direccion = direccion;
            Descripcion = descripcion;
            Telefono = telefono;
            Email = email;
        }
    }
}
