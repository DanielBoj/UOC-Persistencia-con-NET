using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using GenteFit.Models.Enums;
using GenteFit.Models.Collections;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Repositories.Interfaces;
using GenteFit.Models.Repositories.Collections;

namespace GenteFit.Models
{
    public class Horario
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Dia Dia { get; set; }
        public string Hora { get; set; } = null!;
        public Clase Clase { get; set; } = null!;
        // public List<Reserva> Reservas { get; set; } = null!;
        // public List<Espera> Esperas { get; set; } = null!;

        public Horario() {
            Clase = new();
            // Reservas = new();
            // Esperas = new();
        }

        public Horario(Dia dia, string hora, Clase clase)
        {
            Dia = dia;
            Hora = hora;
            Clase = clase;
            // Reservas = new();
            // Esperas = new();
        }
    }
}
