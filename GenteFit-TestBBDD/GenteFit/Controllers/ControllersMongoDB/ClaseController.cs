using GenteFit.Models.Prototypes;
using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenteFit.Controllers.ControllersMongoDB
{
    public class ClaseController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IClase db = new ClaseCollection();

        // GET: ClaseController
        public ActionResult Index()
        {
            // Obtenemos los datos desde MongoDB
            var clases = db.GetAllClases();

            return View(clases);
        }

        // GET: ClaseController/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var clase = db.GetClaseById(id);

                return View(clase);
            } catch { return View(); }
        }

        // GET: ClaseController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClaseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var clase = new Clase()
                {
                    Nombre = collection["Nombre"],
                    Descripcion = collection["Descripcion"],
                    Profesor = collection["Profesor"],
                    Duracion = int.Parse(collection["Duracion"]),
                    Plazas = int.Parse(collection["Plazas"]),
                };

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                db.InsertClase(clase);
                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: ClaseController/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var clase = db.GetClaseById(id);

                return View(clase);
            }
            catch { return View(); }
        }

        // POST: ClaseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var clase = new Clase()
                {
                    // Como se trata de una modificación, debemos usar el ID del objeto realizando la conversión al tipo de datos correcto de Mongo
                    Id = new MongoDB.Bson.ObjectId(id),
                    Nombre = collection["Nombre"],
                    Descripcion = collection["Descripcion"],
                    Profesor = collection["Profesor"],
                    Duracion = int.Parse(collection["Duracion"]),
                    Plazas = int.Parse(collection["Plazas"]),
                };

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                db.UpdateClase(clase);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: ClaseController/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var clase = db.GetClaseById(id);

                return View(clase);
            }
            catch { return View(); }
        }

        // POST: ClaseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                db.DeleteClase(id);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }
    }
}
