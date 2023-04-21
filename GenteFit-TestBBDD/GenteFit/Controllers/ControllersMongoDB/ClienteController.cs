using GenteFit.Models.Prototypes;
using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Usuarios;
using GenteFit.Models.Enums;
using GenteFit.Models.Collections;

namespace GenteFit.Controllers.ControllersMongoDB
{
    //[Route("api/[controller]")]
    //[ApiController]
    /* Controlador DTO para la clase Cliente y el DAO ClienteCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class ClienteController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private ICliente db = new ClienteCollection();

        // GET
        //[HttpGet]
        public async Task<List<Cliente>> GetAllClientes() => await db.GetAllClientes();
        /*{
            try
            {
                // Capturamos los elementos obtenidos de la DB.
                List<Cliente> clientes = await db.GetAllClientes();

                return Ok(clientes);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // GET: ClienteController/Details/5
        //[HttpGet("{id}"), Route("api/[controller]detail/{id}")]
        public async Task<Cliente> Details(string id) => await db.GetClienteById(id);
       /* {
            if (id == null)
            {
                    return BadRequest(ModelState);
            }

            if (db.GetClienteById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Cliente cliente = await db.GetClienteById(id);

                return Ok(cliente);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }
*/
        // POST: ClienteController/Create
        //[HttpPost]
        public async Task<bool> Create(Cliente cliente) => await db.InsertCliente(cliente);
        /*{
            if (cliente == null)
            {
                return BadRequest(ModelState);
            }

            try
            {

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                await db.InsertCliente(cliente);

                return Created("Id", cliente.Id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: ClienteController/Edit/5
        //[HttpPut("{id}")]
        public async Task<bool> Edit(Cliente cliente) => await db.UpdateCliente(cliente);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (cliente == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetClienteById(id) == null)
            {
                return NotFound();
            }

            try
            {

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                await db.UpdateCliente(cliente);

                return Created("Id", id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: ClienteController/Delete/5
        //[HttpDelete("{id}")]
        public async Task<bool> Delete(string id) => await db.DeleteCliente(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetClienteById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                await db.DeleteCliente(id);

                return StatusCode(200);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // Comprobamos si existen documentos en la colección.
        public async Task<bool> IsEmpty() => await db.IsEmpty();
    }
}
