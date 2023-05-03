using GenteFit.Models;

// Las interfaces definen las firmas de los métodos que implementarán los objetos DAO.
namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IEspera
    {
        Task<bool> InsertEspera(Espera espera);
        Task<bool> UpdateEspera(Espera espera);
        Task<bool> DeleteEspera(string id);
        Task<List<Espera>> GetAllEsperas();
        Task<Espera> GetEsperaById(string id);
    }
}
