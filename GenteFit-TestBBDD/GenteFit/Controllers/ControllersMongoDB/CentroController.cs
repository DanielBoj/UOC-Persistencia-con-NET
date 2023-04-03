using GenteFit.Models;
using GenteFit.Models.Prototypes;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenteFit.Controllers.ControllersMongoDB
{
    public class CentroController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private ICentro db = new CentroCollection();

        // GET: CentroController
        public ActionResult Index()
        {
            try
            {
                // Obtenemos los datos desde MongoDB
                var centros = db.GetAllCentros();

                return View(centros);
            } catch
            {
                return View();
            }
        }

        // GET: CentroController/Details/5 -> Devuelve una vista centrada en único documento de la colección
        public ActionResult Details(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var centro = db.GetCentroById(id);

                return View(centro);
            } catch
            {
                return View();
            }
        }

        // GET: CentroController/Create -> Devuelve la vista con el formulario, la aplicación navega a nuestra vista
        public ActionResult Create()
        {
            return View();
        }

        // POST: CentroController/Create -> Insertamos el documento en MongoDB
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var centro = new Centro()
                {
                    Nombre = collection["Nombre"],
                    Descripcion = collection["Descripcion"],
                    Direccion = new Direccion(
                        collection["Direccion.Domicilio"],
                        collection["Direccion.Poblacion"],
                        int.Parse(collection["Direccion.Cp"]),
                        collection["Direccion.Pais"]
                        ),
                    Telefono = collection["Telefono"],
                    Email = collection["Email"],
                };

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                db.InsertCentro(centro);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CentroController/Edit/5 -> Devuelve la vista
        public ActionResult Edit(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var centro = db.GetCentroById(id);

                return View(centro);
            }
            catch
            {
                return View();
            }
        }

        // POST: CentroController/Edit/5 -> Implementa la acción para editar un documento de la colección
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var centro = new Centro()
                {
                    // Como se trata de una modificación, debemos usar el ID del objeto realizando la conversión al tipo de datos correcto de Mongo
                    Id = new MongoDB.Bson.ObjectId(id),
                    Nombre = collection["Nombre"],
                    Descripcion = collection["Descripcion"],
                    Direccion = new Direccion(
                        collection["Direccion.Domicilio"],
                        collection["Direccion.Poblacion"],
                        int.Parse(collection["Direccion.Cp"]),
                        collection["Direccion.Pais"]
                        ),
                    Telefono = collection["Telefono"],
                    Email = collection["Email"],
                };

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                db.UpdateCentro(centro);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CentroController/Delete/5 -> Devuelve la vista
        public ActionResult Delete(string id)
        {
            try 
            { 
            // Recuperamos los valores del documento de la colección a través de su ID a través de los parámetros de la URL
            var centro = db.GetCentroById(id);

            return View(centro);
            }
            catch
            {
                return View();
            }
        }

        // POST: CentroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // Llamamo al método para borrar el elemento en MongoDB
                db.DeleteCentro(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
