using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GenteFit.Models
{
    public class Clase
    {
        // Necesitamos usar un tipo de objeto propio de Mongo para que genere automáticamente los ID únicos de los documentos
        [BsonId]
        public ObjectId Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Profesor { get; set; }
        public int Duracion { get; set; }
        public int Plazas { get; set; }

        private static int TotalClases = 0;

        public Clase()
        {
            TotalClases++;
        }

        public Clase(string nombre, string descripcion, string profesor, int duracion, int plazas)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Profesor = profesor;
            Duracion = duracion;
            Plazas = plazas;
            
            TotalClases++;
        }
    }
}
