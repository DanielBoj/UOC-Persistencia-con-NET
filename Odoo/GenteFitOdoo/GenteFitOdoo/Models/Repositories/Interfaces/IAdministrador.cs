using GenteFit.Models.Usuarios;

// Las interfaces definen las firmas de los métodos que implementarán los objetos DAO.
namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IAdministrador
    {
        Task<bool> InsertAdministrador(Administrador administrador);
        Task<bool> UpdateAdministrador(Administrador administrador);
        Task<bool> DeleteAdministrador(string id);
        Task<List<Administrador>> GetAllAdministradores();
        Task<Administrador> GetAdministradorById(string id);
        Task<bool> IsEmpty();
    }
}
