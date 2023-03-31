using GenteFit.Models.Usuarios;

namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IAdministrador
    {
        bool InsertAdministrador(Administrador administrador);
        bool UpdateAdministrador(Administrador administrador);
        bool DeleteAdministrador(string id);
        List<Administrador> GetAllAdministradores();
        Administrador GetAdministradorById(string id);
    }
}
