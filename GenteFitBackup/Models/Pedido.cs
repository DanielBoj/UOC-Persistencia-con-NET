using Microsoft.VisualBasic;
using ZstdSharp.Unsafe;

namespace GenteFitBackup.Models
{
    /* Esta clase implementa el modelo de entidad de pedido */
    public class Pedido
    {
        public int? Id { get; set; }
        public string? DisplayName { get; set; }
        public DateTime DateOrder { get; set; }
        public string? Company { get; set; }
        public string? Partner { get; set; }
        public double? AmountUntaxed { get; set; }
        public double? AmountTotal { get; set; }
        public string? TaxGroupName { get; set; }
        public string DeliveryStatus { get; set; }


        // Constructor vacío
        public Pedido()
        {
        }
    }
}
