using GenteFit.Models.Prototypes;
using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Usuarios;
using GenteFit.Models.Enums;
using GenteFit.Models.Collections;
using System.Data;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace GenteFit.Controllers.ControllersMongoDB
{
    public class ClienteController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private ICliente db = new ClienteCollection();

        // GET: ClienteController
        public ActionResult Index()
        {
            try
            {
                // Obtenemos los datos desde MongoDB
                var clientes = db.GetAllClientes();

                return View(clientes);
            } catch (Exception ex) { Console.Write(ex.Message.ToString());  return View();  }
            
        }

        // GET: ClienteController/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var cliente = db.GetClienteById(id);

                return View(cliente);
            } catch { return View(); }
        }

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var cliente = new Cliente()
                {
                    Email = collection["Email"],
                    Pass = collection["Pass"],
                    Nombre = collection["Nombre"],
                    Nif = collection["Nif"],
                    Direccion = new Direccion(),
                    Telefono = collection["Telefono"],
                    Genero = (Genero)Enum.Parse(typeof(Genero), collection["Genero"]),
                    Iban = collection["Iban"],
                    Reservas = new Listas<Reserva>(),
                    Esperas = new Colas<Espera>()
                };

                // Guardamos el documento en nuestra colección de MongoDB, esto crea automáticamente el documento, usando el método definido en nuestro Collection.
                db.InsertCliente(cliente);
                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: ClienteController/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var cliente = db.GetClienteById(id);

                return View(cliente);
            }
            catch { return View(); }
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // Creamos un objeto para almacenar todos los datos que capturamos del formulario, debemos parsear todos los campos que no sean strings
                var cliente = new Cliente()
                {
                    // Como se trata de una modificación, debemos usar el ID del objeto realizando la conversión al tipo de datos correcto de Mongo
                    Id = new MongoDB.Bson.ObjectId(id),
                    Email = collection["Email"],
                    Pass = collection["Pass"],
                    Nombre = collection["Nombre"],
                    Nif = collection["Nif"],
                    Direccion = new Direccion(),
                    Telefono = collection["Telefono"],
                    Genero = (Genero)Enum.Parse(typeof(Genero), collection["Genero"]),
                    Iban = collection["Iban"],
                    // Todo -> Modificar las reservas y esperas
                };

                // Llamamos al método Update. El framework se encarga de buscar el objeto.
                db.UpdateCliente(cliente);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }

        // GET: ClienteController/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                var cliente = db.GetClienteById(id);

                return View(cliente);
            }
            catch { return View(); }
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // LLamamos al método para borrar el elemento en MongoDB
                db.DeleteCliente(id);

                return RedirectToAction(nameof(Index));
            }
            catch { return View(); }
        }
    }
}
