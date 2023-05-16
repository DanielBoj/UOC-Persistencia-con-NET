using GenteFit.Models;
using GenteFit.Models.Collections;

// Las interfaces definen las firmas de los métodos que implementarán los objetos DAO.
namespace GenteFit.Models.Repositories.Interfaces
{
    public interface ICentro
    {
        Task<bool> InsertCentro(Centro centro);
        Task<bool> UpdateCentro(Centro centro);
        Task<bool> DeleteCentro(string id);
        Task<List<Centro>> GetAllCentros();
        Task<Centro> GetCentroById(string id);
    }
}
