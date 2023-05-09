using GenteFit.Models;

// Las interfaces definen las firmas de los métodos que implementarán los objetos DAO.
namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IClase
    {
        Task<bool> InsertClase(Clase clase);
        Task<bool> UpdateClase(Clase clase);
        Task<bool> DeleteClase(string id);
        Task<List<Clase>> GetAllClases();
        Task<Clase> GetClaseById(string id);
    }
}
