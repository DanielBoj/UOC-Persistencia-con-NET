using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using GenteFit.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GenteFit.Controllers.ControllersMongoDB
{
    //[Route("api/[controller]")]
    //[ApiController]
    /* Controlador DTO para la clase Administrador y el DAO AdministradorCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class AdministradorController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IAdministrador db = new AdministradorCollection();

        // GET: AdministradorController
        //[HttpGet]
        public async Task<List<Administrador>> GetAllAdministradores() => await db.GetAllAdministradores();
        /*{
            try
            {
                // Capturamos los elementos obtenidos de la DB.
                List<Administrador> administradores = await db.GetAllAdministradores();

                return Ok(administradores);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // GET: AdministradorController/Details/5
        //[HttpGet("{id}"), Route("api/[controller]detail/{id}")]
        public async Task<Administrador> Details(string id) => await db.GetAdministradorById(id);
        /*{
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Administrador administrador = await db.GetAdministradorById(id);

                return Ok(administrador);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // POST: AdministradorController/Create
        //[HttpPost]
        public async Task<bool> Create(Administrador administrador) => await db.InsertAdministrador(administrador);
        /*{
            if (administrador == null)
                {
                    return BadRequest();
                }

            try
            {
                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                await db.InsertAdministrador(administrador);
                return Created("Id", administrador.Id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message); 
            }
        }*/

        // POST: AdministradorController/Edit/5
        //[HttpPut("{id}")]
        public async Task<bool> Edit(Administrador administrador) => await db.UpdateAdministrador(administrador);
        /*{
            if (administrador == null)
            {
                return BadRequest(ModelState);
            }

            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetAdministradorById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // Nos aseguramos de que el ID del objeto que estamos editando es el mismo que el que hemos pasado por parámetro
                administrador.Id = new MongoDB.Bson.ObjectId(id);

                await db.UpdateAdministrador(administrador);

                return Created("Id", id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: AdministradorController/Delete/5
        //[HttpDelete("{id}")]
        public async Task<bool> Delete(string id) => await db.DeleteAdministrador(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetAdministradorById(id) == null)
            {
                return NotFound();
            }
            try
            {
                // Llamamos al método para borrar el elemento en MongoDB
                await db.DeleteAdministrador(id);

                return StatusCode(200);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // Comprobamos si existen documentos
        public async Task<bool> IsEmpty() => await db.IsEmpty();
    }
}
