using GenteFit.Models.Usuarios;

namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IEmpleado
    {
        Task<bool> InsertEmpleado(Empleado empleado);
        Task<bool> UpdateEmpleado(Empleado empleado);
        Task<bool> DeleteEmpleado(string id);
        Task<List<Empleado>> GetAllEmpleados();
        Task<Empleado> GetEmpleadoById(string id);
        Task<bool> IsEmpty();
    }
}
