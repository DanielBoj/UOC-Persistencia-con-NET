namespace GenteFitOdoo.Models
{
    public class Producto
    {
        public int? Id { get; set; }
        public string? DefaultCode { get; set; }
        public string? Name { get; set; }
        public string? Categ { get; set; }
        public decimal? ListPrice { get; set; }
        public decimal? StandardPrice { get; set; }

        // Constructor vacío
        public Producto()
        {
        }


    }
}
