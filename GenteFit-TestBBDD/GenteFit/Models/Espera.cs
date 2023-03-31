using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using GenteFit.Models.Usuarios;

namespace GenteFit.Models
{
    public class Espera
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        public ObjectId Id { get; set; }
        public Cliente Cliente { get; set; }
        public Horario Horario { get; set; }

        public Espera() { }

        public Espera(Cliente cliente, Horario horario)
        {
            Cliente = cliente;
            Horario = horario;
        }
    }
}
