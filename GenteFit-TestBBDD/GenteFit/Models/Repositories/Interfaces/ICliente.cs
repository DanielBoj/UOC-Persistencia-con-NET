using GenteFit.Models.Usuarios;

namespace GenteFit.Models.Repositories.Interfaces
{
    public interface ICliente
    {
        bool InsertCliente(Cliente cliente);
        bool UpdateCliente(Cliente cliente);
        bool DeleteCliente(string id);
        List<Cliente> GetAllClientes();
        Cliente GetClienteById(string id);
    }
}
