using GenteFit.Models.Prototypes;
using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Usuarios;

namespace GenteFit.Controllers.ControllersMongoDB
{
    public class EsperaController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IEspera db = new EsperaCollection();

        // GET: EsperaController
        public ActionResult Index()
        {
            try
            {
                // Obtenemos los datos desde MongoDB
                var esperas = db.GetAllEsperas();

                return View(esperas);
            } catch
            {
                return View();
            }
        }

        // GET: EsperaController/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var espera = db.GetEsperaById(id);

                return View(espera);
            } catch
            {
                return View();
            }
        }

        // GET: EsperaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EsperaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var espera = new Espera()
                {
                    Cliente = new Cliente(),
                    Horario = new Horario()
                };

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                db.InsertEspera(espera);
                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: EsperaController/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var espera = db.GetEsperaById(id);

                return View(espera);
            } catch { return View(); }
        }

        // POST: EsperaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var espera = new Espera()
                {
                    // Como se trata de una modificación, debemos usar el ID del objeto realizando la conversión al tipo de datos correcto de Mongo
                    Id = new MongoDB.Bson.ObjectId(id),
                    // Todo -> Añadir los objetos a la lista.
                };

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                db.UpdateEspera(espera);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: EsperaController/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var espera = db.GetEsperaById(id);

                return View(espera);
            }
            catch { return View(); }
        }

        // POST: EsperaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                db.DeleteEspera(id);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }
    }
}
