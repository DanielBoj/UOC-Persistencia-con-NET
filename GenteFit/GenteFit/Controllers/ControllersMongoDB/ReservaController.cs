using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using GenteFit.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace GenteFit.Controllers.ControllersMongoDB
{
    //[Route("api/[controller]")]
    //[ApiController]
    /* Controlador DTO para la clase Reserva y el DAO ReservaCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class ReservaController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IReserva db = new ReservaCollection();
        //private ICliente cliente = new ClienteCollection();
        //private IHorario horario = new HorarioCollection();

        // GET
        //[HttpGet]
        public async Task<List<Reserva>> GetAllReservas() => await db.GetAllReservas();
        /*{
            try
            {
                // Capturamos los elementos obtenidos de la DB.
                List<Reserva> reservas = await db.GetAllReservas();

                return Ok(reservas);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // GET: ReservaController/Details/5
        //[HttpGet("{id}"), Route("api/[controller]detail/{id}")]
        public async Task<Reserva> Details(string id) => await db.GetReservaById(id);
       /* {
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetReservaById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Reserva reserva = await db.GetReservaById(id);

                return Ok(reserva);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }*/

        // POST: ReservaController/Create
        //[HttpPost]
        public async Task<bool> Create(Reserva reserva) => await db.InsertReserva(reserva);
        /*{
            if (reserva == null)
            {
                return BadRequest(ModelState);
            }

            try
            { 
                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                await db.InsertReserva(reserva);
                return Created("Id", reserva.Id);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/

        // POST: ReservaController/Edit/5
        //[HttpPut("{id}")]
        public async Task<bool> Edit(Reserva reserva) => await db.UpdateReserva(reserva);
        /* {
             if (id == null)
             {
                 return BadRequest(ModelState);
             }

             if (reserva == null)
             {
                 return BadRequest(ModelState);
             }

             if (db.GetReservaById(id) == null)
             {
                 return NotFound();
             }

             try
             {
                 // Nos aseguramos de que el ID del objeto sea el correcto.
                 reserva.Id = new MongoDB.Bson.ObjectId(id);

                 // Llamamos al método Update. El framework se encarga de buscar el objeto.
                 await db.UpdateReserva(reserva);

                 return Created("Id", id);
             }
             catch (Exception err)
             {
                 return StatusCode(400, err.Message);
             }
         }*/

        // POST: ReservaController/Delete/5
        //[HttpDelete("{id}")]
        public async Task<bool> Delete(string id) => await db.DeleteReserva(id);
        /*{
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            if (db.GetReservaById(id) == null)
            {
                return NotFound();
            }

            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                await db.DeleteReserva(id);

                return StatusCode(200);
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }*/
    }
}
