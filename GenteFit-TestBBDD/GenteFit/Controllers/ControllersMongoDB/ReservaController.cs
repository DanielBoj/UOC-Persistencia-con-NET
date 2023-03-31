using GenteFit.Models;
using GenteFit.Models.Collections;
using GenteFit.Models.Enums;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using GenteFit.Models.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenteFit.Controllers.ControllersMongoDB
{
    public class ReservaController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IReserva db = new ReservaCollection();

        // GET: ReservaController
        public ActionResult Index()
        {
            try
            {
                // Obtenemos los datos desde MongoDB
                var reservas = db.GetAllReservas();

                return View(reservas);
            } catch { return View(); }
        }

        // GET: ReservaController/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var reserva = db.GetReservaById(id);

                return View(reserva);
            } catch
            {
                return View();
            }
        }

        // GET: ReservaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReservaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var reserva = new Reserva()
                {
                    Cliente = new Cliente(),
                    Horario = new Horario()
                };

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                db.InsertReserva(reserva);
                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: ReservaController/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var reserva = db.GetReservaById(id);

                return View(reserva);
            }
            catch
            {
                return View();
            }
        }

        // POST: ReservaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var reserva = new Reserva()
                {
                    // Como se trata de una modificación, debemos usar el ID del objeto realizando la conversión al tipo de datos correcto de Mongo
                    Id = new MongoDB.Bson.ObjectId(id),
                    // TODO -> Añadir objetos a las listas
                };

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                db.UpdateReserva(reserva);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: ReservaController/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var reserva = db.GetReservaById(id);

                return View(reserva);
            }
            catch
            {
                return View();
            }
        }

        // POST: ReservaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                db.DeleteReserva(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
