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
        public async Task<List<Cliente>> GetAllClientes() => await _api.GetAllClientes();

        // Creamos un cliente en la BBDD de Odoo
        public async Task<int> CreateCliente(Cliente cliente) => await _api.CreateCliente(cliente);

        // Método para obtener todos los productos de la BBDD de Odoo
        public async Task<List<Producto>> GetAllProductos() => await _api.GetAllProductos();

        // Creamos un producto en la BBDD de Odoo
        public async Task<int> CreateProducto(Producto producto) => await _api.CreateProducto(producto);

        // Método para obtener todos los proveedores
        public async Task<List<Proveedor>> GetAllProveedores() => await _api.GetAllProveedores();

        // Creamos un proveedor en la BBDD de Odoo
        public async Task<int> CreateProveedor(Proveedor proveedor) => await _api.CreateProveedor(proveedor);
    }
}
