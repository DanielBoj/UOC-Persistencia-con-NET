using GenteFit.Models.Usuarios;
using GenteFitOdoo.Models;
using GenteFitOdoo.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GenteFitOdoo.Controllers.Controllers.ControllersPython
{
    public class PythonApiController : ControllerBase
    {
        // Instanciamos la clase de conexión a la API de Python
        private readonly PythonApiConnection _api;

        // Constructor
        public PythonApiController()
        {
            HttpClient client = new();
            _api = new PythonApiConnection(client);
        }

        // Método para obtener todos los usuarios de la BBDD de Odoo
        [HttpGet("odoo/clientes")]
        public async Task<List<Cliente>> GetAllClientes() => await _api.GetAllClientes();

        // Método para obtener todos los productos de la BBDD de Odoo
        [HttpGet("odoo/productos")]
        public async Task<List<Producto>> GetAllProductos() => await _api.GetAllProductos();

        // Método para obtener todos los proveedores
        [HttpGet("odoo/proveedores")]
        public async Task<List<Proveedor>> GetAllProveedores() => await _api.GetAllProveedores();
    }
}
