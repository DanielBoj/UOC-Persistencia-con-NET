using GenteFit.Models.Prototypes;
using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenteFitOdoo.Controllers.Controllers.ControllersMongoDB
{
    /* Controlador DTO para la clase Centro y el DAO CentroCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class ClaseController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IClase db = new ClaseCollection();

        public async Task<List<Clase>> GetAllClases() => await db.GetAllClases();

        public async Task<Clase> Details(string id) => await db.GetClaseById(id);

        public async Task<bool> Create(Clase clase) => await db.InsertClase(clase);

        public async Task<bool> Edit(Clase clase) => await db.UpdateClase(clase);

        public async Task<bool> Delete(string id) => await db.DeleteClase(id);
    }
}
