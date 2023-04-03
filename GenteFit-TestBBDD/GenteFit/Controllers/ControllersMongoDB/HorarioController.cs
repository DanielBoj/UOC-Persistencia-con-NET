using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Enums;
using GenteFit.Models.Collections;
using System.Dynamic;

namespace GenteFit.Controllers.ControllersMongoDB
{
    public class HorarioController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IHorario db = new HorarioCollection();
        private IClase clase = new ClaseCollection();

        // GET: HorarioController
        public ActionResult Index()
        {
            // Obtenemos los datos desde MongoDB
            var horarios = db.GetAllHorarios();

            return View(horarios);
        }

        // GET: HorarioController/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var horario = db.GetHorarioById(id);

                return View(horario);
            } catch
            {
                return View();
            }
        }

        // GET: HorarioController/Create
        public ActionResult Create()
        {
            try
            {
                var clases = clase.GetAllClases();

                ViewData["Clases"] = clases;

                return View();
            } catch { return View();  }
            
        }

        // POST: HorarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var horario = new Horario()
                {
                    Clase = clase.GetClaseById(collection["Clase"]),
                    Dia = (Dia) Enum.Parse(typeof(Dia), collection["Dia"]),
                    Hora = collection["Hora"].ToString(),
                    Reservas = new Listas<Reservas>(),
                    Esperas = new Colas<Espera>()
                };

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                db.InsertHorario(horario);
                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: HorarioController/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var horario = db.GetHorarioById(id);

                return View(horario);
            }
            catch { return View(); }
        }

        // POST: HorarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var horario = new Horario()
                {
                    // Como se trata de una modificación, debemos usar el ID del objeto realizando la conversión al tipo de datos correcto de Mongo
                    Id = new MongoDB.Bson.ObjectId(id),
                    Clase = clase.GetClaseById(collection["Clase"]),
                    Dia = (Dia)Enum.Parse(typeof(Dia), collection["Dia"]),
                    Hora = collection["Hora"].ToString(),
                    // TODO -> Añadir objetos a las listas
                };

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                db.UpdateHorario(horario);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: HorarioController/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var horario = db.GetHorarioById(id);

                return View(horario);
            }
            catch { return View(); }
        }

        // POST: HorarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                db.DeleteHorario(id);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }
    }
}
