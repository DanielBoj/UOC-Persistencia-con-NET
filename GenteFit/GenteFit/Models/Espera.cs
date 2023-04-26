using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using GenteFit.Models.Usuarios;

namespace GenteFit.Models
{
    public class Espera
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Cliente Cliente { get; set; } = null!;
        public Horario Horario { get; set; } = null!;

        public Espera() {
            Cliente = new();
            Horario = new();
        }

        public Espera(Cliente cliente, Horario horario)
        {
            Cliente = cliente;
            Horario = horario;
        }
    }
}
