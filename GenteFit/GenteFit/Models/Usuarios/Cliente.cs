using GenteFit.Models.Enums;
using GenteFit.Models.Prototypes;
using MongoDB.Bson.Serialization.Attributes;

namespace GenteFit.Models.Usuarios
{
    public class Cliente : User
    {
        // Regla de negocio: Un cliente solamente puede tener 2 reservas.
        public const int MaxReservas = 2;

        [BsonElement("Nombre")]
        public string Nombre { get; set; } = null!;
        [BsonElement("Nif")]
        public string Nif { get; set; } = null!;
        [BsonElement("Direccion")]
        public Direccion Direccion { get; set; } = null!;
        [BsonElement("Telefono")]
        public string Telefono { get; set; } = null!;
        [BsonElement("Genero")]
        public Genero Genero { get; set; }
        [BsonElement("Iban")]
        public string Iban { get; set; } = null!;

        public Cliente() {
            Direccion = new();
        }

        public Cliente(string email, string pass, string nombre, string nif, Direccion direccion, string telefono, Genero genero, string iban) : base(email, pass)
        {
            Nombre = nombre;
            Nif = nif;
            Direccion = direccion;
            Telefono = telefono;
            Genero = genero;
            Iban = iban;
            //Reservas = new();
            //Esperas = new();
        }

    }
}
