using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Usuarios;

namespace GenteFit.Controllers.ControllersMongoDB
{
    //[Route("api/[controller]")]
    //[ApiController]
    /* Controlador DTO para la clase Empleado y el DAO EmpleadoCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class EmpleadoController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IEmpleado db = new EmpleadoCollection();

        //GET
        //[HttpGet]
        public async Task<List<Empleado>> GetAllEmpleados() => await db.GetAllEmpleados();
        /*{
            try
            {
                // Capturamos los elementos obtenidos de la DB.
                List<Empleado> empleados = await db.GetAllEmpleados();

                return Ok(empleados);
            }
            catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // GET: EmpleadoController/Details/5
        //[HttpGet("{id}"), Route("api/[controller]detail/{id}")]
        public async Task<Empleado> Details(string id) => await db.GetEmpleadoById(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetEmpleadoById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Empleado empleado = await db.GetEmpleadoById(id);

                return Ok(empleado);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }
*/
        // POST: EmpleadoController/Create
        //[HttpPost]
        public async Task<bool> Create(Empleado empleado) => await db.InsertEmpleado(empleado);
        /*{
            if (empleado == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                await db.InsertEmpleado(empleado);

                return Created("Id", empleado.Id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: EmpleadoController/Edit/5
        //[HttpPut("{id}")]
        public async Task<bool> Edit(Empleado empleado) => await db.UpdateEmpleado(empleado);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (empleado == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetEmpleadoById(id) == null)
            {
                return NotFound();
            }

            try

            {
                // Nos aseguramos de que el ID del objeto sea el correcto.
                empleado.Id = new MongoDB.Bson.ObjectId(id);

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                await db.UpdateEmpleado(empleado);

                return Created("Id", id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: EmpleadoController/Delete/5
        //[HttpDelete("{id}")]
        public async Task<bool> Delete(string id) => await db.DeleteEmpleado(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetEmpleadoById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                await db.DeleteEmpleado(id);

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
