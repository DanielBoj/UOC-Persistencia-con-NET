using GenteFit.Models.Usuarios;

namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IEmpleado
    {
        bool InsertEmpleado(Empleado empleado);
        bool UpdateEmpleado(Empleado empleado);
        bool DeleteEmpleado(string id);
        List<Empleado> GetAllEmpleados();
        Empleado GetEmpleadoById(string id);
    }
}
