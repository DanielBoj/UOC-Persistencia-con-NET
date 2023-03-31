using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using GenteFit.Models.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenteFit.Controllers.ControllersMongoDB
{
    public class AdministradorController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IAdministrador db = new AdministradorCollection();

        // GET: AdministradorController
        public ActionResult Index()
        {
            try
            {
                // Obtenemos los datos desde MongoDB
                var administradores = db.GetAllAdministradores();

                return View(administradores);
            } catch
            {
                return View();
            }
            
        }

        // GET: AdministradorController/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var administrador = db.GetAdministradorById(id);

                return View(administrador);
            } catch
            {
                return View();
            }
        }

        // GET: AdministradorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdministradorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var administrador = new Administrador()
                {
                    Email = collection["Email"],
                    Pass = collection["Pass"],
                };

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                db.InsertAdministrador(administrador);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdministradorController/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var administrador = db.GetAdministradorById(id);

                return View(administrador);
            }
            catch
            {
                return View();
            }
        }

        // POST: AdministradorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var administrador = new Administrador()
                {
                    Id = new MongoDB.Bson.ObjectId(id),
                    Email = collection["Email"],
                    Pass = collection["Pass"],
                };

                db.UpdateAdministrador(administrador);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdministradorController/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var administrador = db.GetAdministradorById(id);

                return View(administrador);
            }
            catch
            {
                return View();
            }
        }

        // POST: AdministradorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // Llamamo al método para borrar el elemento en MongoDB
                db.DeleteAdministrador(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
