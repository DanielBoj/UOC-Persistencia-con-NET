namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IReserva
    {
        bool InsertReserva(Reserva reserva);
        bool UpdateReserva(Reserva reserva);
        bool DeleteReserva(string id);
        List<Reserva> GetAllReservas();
        Reserva GetReservaById(string id);
    }
}
