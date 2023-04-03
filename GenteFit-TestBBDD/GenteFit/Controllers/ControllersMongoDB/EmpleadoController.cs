using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Usuarios;

namespace GenteFit.Controllers.ControllersMongoDB
{
    public class EmpleadoController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IEmpleado db = new EmpleadoCollection();

        // GET: EmpleadoController
        public ActionResult Index()
        {
            try
            {
                // Obtenemos los datos desde MongoDB
                var empleados = db.GetAllEmpleados();

                return View(empleados);
            } catch
            {
                return View();
            }
        }

        // GET: EmpleadoController/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var empleado = db.GetEmpleadoById(id);

                return View(empleado);
            } catch { return View(); }
        }

        // GET: EmpleadoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpleadoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var empleado = new Empleado()
                {
                    Email = collection["Email"],
                    Pass = collection["Passs"]
                };

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                db.InsertEmpleado(empleado);
                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: EmpleadoController/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var empleado = db.GetEmpleadoById(id);

                return View(empleado);
            }
            catch { return View(); }
        }

        // POST: EmpleadoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var empleado = new Empleado()
                {
                    // Como se trata de una modificación, debemos usar el ID del objeto realizando la conversión al tipo de datos correcto de Mongo
                    Id = new MongoDB.Bson.ObjectId(id),
                    Email = collection["Email"],
                    Pass = collection["Pass"],
                };

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                db.UpdateEmpleado(empleado);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: EmpleadoController/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var empleado = db.GetEmpleadoById(id);

                return View(empleado);
            }
            catch { return View(); }
        }

        // POST: EmpleadoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                db.DeleteEmpleado(id);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }
    }
}
