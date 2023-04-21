using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Build.Framework;
using MongoDB.Bson;
using System.Drawing.Printing;

namespace GenteFit.Controllers.ControllersMongoDB
{
    //[Route("api/[controller]")]
    //[ApiController]
    /* Controlador DTO para la clase Centro y el DAO CentroCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class CentroController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private ICentro db = new CentroCollection();

        //GET
        //[HttpGet]
        public async Task<List<Centro>> GetAllCentros() => await db.GetAllCentros();
        /*{
            *//*try
            {
                return await db.GetAllCentros();
                return Ok(centros);
            } catch (Exception err)
            {
                return err.Message;
            }*//*
        }*/

        public async Task<Centro> Details(string id) => await db.GetCentroById(id);
        /*{
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Centro centro = await db.GetCentroById(id);

                if (centro == null)
                {
                    return NotFound();
                }

                return Ok(centro);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // POST: CentroController/Create -> Insertamos el documento en MongoDB
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Centro centro)
        {
            if (centro == null)
            {
                return BadRequest();
            }

            try
            {
                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                await db.InsertCentro(centro);
                return Created("Id", centro.Id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // POST: CentroController/Edit/5 -> Implementa la acción para editar un documento de la colección
        //[HttpPut("{id}")]
        public async Task<bool> Edit(Centro centro) => await db.UpdateCentro(centro);
        /*{
            if (centro == null)
            {
                return BadRequest(ModelState);
            }

            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetCentroById(id) == null)
            {
                return NotFound();
            }

            try {

                // Preparamos el objeto:
                // Nos aseguramos de que el ID del objeto que estamos editando es el mismo que el que hemos pasado por parámetro
                centro.Id = new MongoDB.Bson.ObjectId(id);

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                await db.UpdateCentro(centro);

                return Created("Id", id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: CentroController/Delete/5
        //[HttpDelete("{id}")]
        public async Task<bool> Delete(string id) => await db.DeleteCentro(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetCentroById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // Llamamos al método para borrar el elemento en MongoDB
                await db.DeleteCentro(id);

                return StatusCode(200);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.StackTrace);
            }
        }*/
    }
}
