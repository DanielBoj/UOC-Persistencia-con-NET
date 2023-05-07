using GenteFit.Models.Prototypes;

namespace GenteFitOdoo.Models
{
    public class Proveedor
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Nif { get; set; }
        public Direccion? Direccion { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }

        // Constructor vacío
        public Proveedor()
        {
            Direccion = new();
        }
    }
}
