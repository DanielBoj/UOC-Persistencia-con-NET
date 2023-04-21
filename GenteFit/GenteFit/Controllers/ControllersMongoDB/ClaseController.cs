using GenteFit.Models.Prototypes;
using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenteFit.Controllers.ControllersMongoDB
{
    //[Route("api/[controller]")]
    //[ApiController]
    /* Controlador DTO para la clase Centro y el DAO CentroCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class ClaseController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IClase db = new ClaseCollection();

        // GET
        //[HttpGet]
        public async Task<List<Clase>> GetAllClases() => await db.GetAllClases();
        /*{
           try
            {
                // Capturamos los elementos obtenidos de la DB.
                List<Clase> clases = await db.GetAllClases();

                return Ok(clases);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // GET: ClaseController/Details/5
        //[HttpGet("{id}"), Route("api/[controller]detail/{id}")]
        public async Task<Clase> Details(string id) => await db.GetClaseById(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetClaseById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Clase clase = await db.GetClaseById(id);

                return Ok(clase);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // POST: ClaseController/Create
        //[HttpPost]
        public async Task<bool> Create(Clase clase) => await db.InsertClase(clase);
        /*{
            if (clase == null)
            {
                return BadRequest(ModelState);
            }

            try
            {

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                await db.InsertClase(clase);

                return Created("Id", clase.Id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: ClaseController/Edit/5
        //[HttpPut("{id}")]
        public async Task<bool> Edit(Clase clase) => await db.UpdateClase(clase);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (clase == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetClaseById(id) == null)
            {
                return NotFound();
            }

            try
            {

                // Nos aseguramos de que el ID del objeto que estamos editando es el mismo que el que hemos pasado por parámetro
                clase.Id = new MongoDB.Bson.ObjectId(id);

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                await db.UpdateClase(clase);

                return Created("Id", id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: ClaseController/Delete/5
        //[HttpDelete("{id}")]
        public async Task<bool> Delete(string id) => await db.DeleteClase(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetClaseById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                await db.DeleteClase(id);

                return StatusCode(200);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/
    }
}
