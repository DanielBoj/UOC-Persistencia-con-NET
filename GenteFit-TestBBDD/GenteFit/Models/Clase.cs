﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GenteFit.Models
{
    public class Clase
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? Id { get; set; }
        [BsonElement("Nombre")]
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Profesor { get; set; } = null!;
        public int Duracion { get; set; }
        public int Plazas { get; set; }

        private static int TotalClases { get; set; } = 0;

        public Clase()
        {
        }

        public Clase(string nombre, string descripcion, string profesor, int duracion, int plazas)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Profesor = profesor;
            Duracion = duracion;
            Plazas = plazas;
        }

        // Generamos 2 métodos estáticos para incrementar y decrementar el número de clases.

        public static void AdvanceTotalClases() { TotalClases++;  }

        public static void DecreaseTotalClases() { TotalClases--; }
    }
}
