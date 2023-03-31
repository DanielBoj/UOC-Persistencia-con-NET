using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using GenteFit.Models.Enums;
using GenteFit.Models.Collections;

namespace GenteFit.Models
{
    public class Horario
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        public ObjectId Id { get; set; }
        public Dia Dia { get; set; }
        public TimeOnly Hora { get; set; }
        public Clase Clase { get; set; }
        public Listas<Reservas> Reservas { get; set; }
        public Colas<Espera> Esperas { get; set; }

        public Horario() { }

        public Horario(Dia dia, TimeOnly hora, Clase clase)
        {
            Dia = dia;
            Hora = hora;
            Clase = clase;
            Reservas = new Listas<Reservas>();
            Esperas = new Colas<Espera>();
        }
    }
}
