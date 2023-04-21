using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Enums;
using GenteFit.Models.Collections;
using System.Dynamic;

namespace GenteFit.Controllers.ControllersMongoDB
{
    //[Route("api/[controller]")]
    //[ApiController]
    /* Controlador DTO para la clase Horario y el DAO HorarioCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class HorarioController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IHorario db = new HorarioCollection();
        //private IClase clase = new ClaseCollection();

        // GET
        //[HttpGet]
        public async Task<List<Horario>> GetAllHorarios() => await db.GetAllHorarios();
        /*{
            try
            {
                // Devolvemos una lista de elementos obtenidos desde la BD.
                List<Horario> horarios = await db.GetAllHorarios();

                return Ok(horarios);
            }
            catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // GET: HorarioController/Details/5
        //[HttpGet("{id}"), Route("api/[controller]detail/{id}")]
        public async Task<Horario> Details(string id) => await db.GetHorarioById(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetHorarioById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Horario horario = await db.GetHorarioById(id);

                return Ok(horario);
            }
            catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // POST: HorarioController/Create
        //[HttpPost]
        public async Task<bool> Create(Horario horario) => await db.InsertHorario(horario);
        /*{
            if (horario == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                await db.InsertHorario(horario);

                return Created("Id", horario.Id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: HorarioController/Edit/5
        //[HttpPut("{id}")]
        public async Task<bool> Edit(Horario horario) => await db.UpdateHorario(horario);
       /* {
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (horario == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetHorarioById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // Nos aseguramos de que el ID del objeto sea el correcto.
                horario.Id = new MongoDB.Bson.ObjectId(id);

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                await db.UpdateHorario(horario);

                return Created("Id", id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: HorarioController/Delete/5
        //[HttpDelete("{id}")]
        public async Task<bool> Delete(string id) => await db.DeleteHorario(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetHorarioById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                await db.DeleteHorario(id);

                return StatusCode(200);
            }
            catch (Exception err) { return StatusCode(400, err.Message); }
        }*/
    }
}
