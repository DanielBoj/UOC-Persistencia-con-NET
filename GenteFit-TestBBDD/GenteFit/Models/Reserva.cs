using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using GenteFit.Models.Usuarios;

namespace GenteFit.Models
{
    public class Reserva
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        public ObjectId Id { get; set; }
        public Horario Horario;
        public Cliente Cliente;

        public Reserva() { }

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
