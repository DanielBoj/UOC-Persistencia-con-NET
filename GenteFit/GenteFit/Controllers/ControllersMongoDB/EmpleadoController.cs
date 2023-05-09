using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Usuarios;

namespace GenteFit.Controllers.ControllersMongoDB
{
    /* Controlador DTO para la clase Empleado y el DAO EmpleadoCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class EmpleadoController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IEmpleado db = new EmpleadoCollection();

        public async Task<List<Empleado>> GetAllEmpleados() => await db.GetAllEmpleados();
       
        public async Task<Empleado> Details(string id) => await db.GetEmpleadoById(id);
       
        public async Task<bool> Create(Empleado empleado) => await db.InsertEmpleado(empleado);
        
        public async Task<bool> Edit(Empleado empleado) => await db.UpdateEmpleado(empleado);
       
        public async Task<bool> Delete(string id) => await db.DeleteEmpleado(id);
        
        // Comprobamos si existen documentos en la colección.
        public async Task<bool> IsEmpty() => await db.IsEmpty();
    }
}
