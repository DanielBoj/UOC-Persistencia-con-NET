namespace GenteFit.Models.Collections
{
    public class Reservas : Listas<Reserva>
    {
        private int Ceil { get; set; }
        private bool HasEspera { get; set; }

        public Reservas (int ceil) : base()
        {
            Ceil = ceil;            
            HasEspera = false;
        }

        public override Reserva? Add(Reserva item)
        {
            if (item != null && base.Count() < Ceil)
            {
                return base.Add(item);
            }

            return default;
        }

        public bool IsFull()
        {
            return base.Count() == Ceil;
        }


    }
}
