using GenteFit.Models.Usuarios;

namespace GenteFit.Models.Repositories.Interfaces
{
    public interface ICliente
    {
        Task<bool> InsertCliente(Cliente cliente);
        Task<bool> UpdateCliente(Cliente cliente);
        Task<bool> DeleteCliente(string id);
        Task<List<Cliente>> GetAllClientes();
        Task<Cliente> GetClienteById(string id);
        Task<bool> IsEmpty();
    }
}
