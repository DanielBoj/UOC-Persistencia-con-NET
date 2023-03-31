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
        public ObjectId Id { get; set; }
        [Required(ErrorMessage = "Debe completar este campo.")]
        public string Nombre { get; set; }
        public Direccion Direccion { get; set; }
        [Required(ErrorMessage = "Debe completar este campo.")]
        public string Descripcion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public Centro() { }

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
