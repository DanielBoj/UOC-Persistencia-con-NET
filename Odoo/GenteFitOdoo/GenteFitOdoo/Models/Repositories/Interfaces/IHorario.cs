using GenteFit.Models;

// Las interfaces definen las firmas de los métodos que implementarán los objetos DAO.
namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IHorario
    {
        Task<bool> InsertHorario(Horario horario);
        Task<bool> UpdateHorario(Horario horario);
        Task<bool> DeleteHorario(string id);
        Task<List<Horario>> GetAllHorarios();
        Task<Horario> GetHorarioById(string id);
    }
}
