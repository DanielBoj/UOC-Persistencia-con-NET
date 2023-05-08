using GenteFit.Models;

// Las interfaces definen las firmas de los métodos que implementarán los objetos DAO.
namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IReserva
    {
        Task<bool> InsertReserva(Reserva reserva);
        Task<bool> UpdateReserva(Reserva reserva);
        Task<bool> DeleteReserva(string id);
        Task<List<Reserva>> GetAllReservas();
        Task<Reserva> GetReservaById(string id);
    }
}
