using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using GenteFit.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace GenteFitOdoo.Controllers.Controllers.ControllersMongoDB
{
    /* Controlador DTO para la clase Reserva y el DAO ReservaCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class ReservaController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IReserva db = new ReservaCollection();

        public async Task<List<Reserva>> GetAllReservas() => await db.GetAllReservas();

        public async Task<Reserva> Details(string id) => await db.GetReservaById(id);

        public async Task<bool> Create(Reserva reserva) => await db.InsertReserva(reserva);

        public async Task<bool> Edit(Reserva reserva) => await db.UpdateReserva(reserva);

        public async Task<bool> Delete(string id) => await db.DeleteReserva(id);
    }
}
