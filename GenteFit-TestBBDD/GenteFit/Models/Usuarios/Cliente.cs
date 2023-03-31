using GenteFit.Models.Collections;
using GenteFit.Models.Enums;
using GenteFit.Models.Prototypes;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace GenteFit.Models.Usuarios
{
    public class Cliente : User
    {
        // Regla de negocio: Un cliente solamente puede tener 2 reservas.
        public const int MaxReservas = 2;

        [Required(ErrorMessage="Debe completar este campo.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe completar este campo.")]
        public string Nif { get; set; }
        public Direccion Direccion { get; set; }
        public string Telefono { get; set; }
        public Genero Genero { get; set; }
        [Required(ErrorMessage = "Debe completar este campo.")]
        public string Iban { get; set; }
        public Listas<Reserva> Reservas { get; set; }
        public Colas<Espera> Esperas { get; set; }

        public Cliente() { }

        public Cliente(string email, string pass, string nombre, string nif, Direccion direccion, string telefono, Genero genero, string iban) : base(email, pass)
        {
            Nombre = nombre;
            Nif = nif;
            Direccion = direccion;
            Telefono = telefono;
            Genero = genero;
            Iban = iban;
            Reservas = new Listas<Reserva>();
            Esperas = new Colas<Espera>();
        }

    }
}
