using GenteFit.Models.Prototypes;
using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Usuarios;

namespace GenteFitOdoo.Controllers.Controllers.ControllersMongoDB
{
    /* Controlador DTO para la clase Cliente y el DAO ClienteCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class ClienteController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private ICliente db = new ClienteCollection();

        public async Task<List<Cliente>> GetAllClientes() => await db.GetAllClientes();

        public async Task<Cliente> Details(string id) => await db.GetClienteById(id);

        public async Task<bool> Create(Cliente cliente) => await db.InsertCliente(cliente);

        public async Task<bool> Edit(Cliente cliente) => await db.UpdateCliente(cliente);

        public async Task<bool> Delete(string id) => await db.DeleteCliente(id);

        // Comprobamos si existen documentos en la colección.
        public async Task<bool> IsEmpty() => await db.IsEmpty();
    }
}
