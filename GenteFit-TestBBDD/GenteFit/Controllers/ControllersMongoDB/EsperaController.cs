using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Usuarios;

namespace GenteFit.Controllers.ControllersMongoDB
{
    //[Route("api/[controller]")]
    //[ApiController]
    /* Controlador DTO para la clase Espera y el DAO EsperaCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class EsperaController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IEspera db = new EsperaCollection();
        //private ICliente cliente = new ClienteCollection();
        //private IHorario horario = new HorarioCollection();

        // GET
        //[HttpGet]
        public async Task<List<Espera>> GetAllEsperas() => await db.GetAllEsperas();
        /*{
            try
            {
                // Capturamos los elementos obtenidos de la DB.
                List<Espera> esperas = await db.GetAllEsperas();

                return Ok(esperas);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // GET: EsperaController/Details/5
        //[HttpGet("{id}"), Route("api/[controller]detail/{id}")]
        public async Task<Espera> Details(string id) => await db.GetEsperaById(id);
        /*{
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Espera espera = await db.GetEsperaById(id);

                return Ok(espera);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // POST: EsperaController/Create
        //[HttpPost]
        public async Task<bool> Create(Espera espera) => await db.InsertEspera(espera);
        /*{
            if (espera == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                *//*var espera = new Espera()
                {
                    Id = new MongoDB.Bson.ObjectId(collection["Cliente.id"]),
                    //Cliente = cliente.GetClienteById(collection["Cliente"]),
                    //Horario = horario.GetHorarioById(collection["Horario"])
                };*//*

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                await db.InsertEspera(espera);

                return Created("Id", espera.Id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: EsperaController/Edit/5
        //[HttpPut("{id}")]
        public async Task<bool> Edit(Espera espera) => await db.UpdateEspera(espera);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (espera == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetEsperaById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // Nos aseguramos de que el ID del objeto sea el correcto.
                espera.Id = new MongoDB.Bson.ObjectId(id);

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                await db.UpdateEspera(espera);

                return Created("Id", id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: EsperaController/Delete/5
        //[HttpDelete("{id}")]
        public async Task<bool> Delete(string id) => await db.DeleteEspera(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetEsperaById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                await db.DeleteEspera(id);

                return StatusCode(200);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/
    }
}
