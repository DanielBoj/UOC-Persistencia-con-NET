using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using GenteFit.Models.Usuarios;

namespace GenteFit.Models
{
    public class Reserva
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Horario"), BsonDefaultValue(null), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public Horario Horario { get; set; } = null!;
        [BsonElement("Cliente"), BsonDefaultValue(null), BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public Cliente Cliente { get; set; } = null!;

        public Reserva() {
            Horario = new();
            Cliente = new();
        }

        public Reserva(Horario horario, Cliente cliente)
        {
            Horario = horario;
            Cliente = cliente;
        }

        // TODO => Override ToString

        public bool Equals(Reserva reserva)
        {
            return reserva.Id.Equals(this.Id);
        }
    }
}
