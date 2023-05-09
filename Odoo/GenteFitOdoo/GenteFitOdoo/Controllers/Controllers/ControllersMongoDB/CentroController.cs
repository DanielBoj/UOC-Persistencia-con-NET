using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenteFitOdoo.Controllers.Controllers.ControllersMongoDB
{
    /* Controlador DTO para la clase Centro y el DAO CentroCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class CentroController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private ICentro db = new CentroCollection();

        public async Task<List<Centro>> GetAllCentros() => await db.GetAllCentros();

        public async Task<Centro> Details(string id) => await db.GetCentroById(id);

        public async Task<bool> Create(Centro centro) => await db.InsertCentro(centro);

        public async Task<bool> Edit(Centro centro) => await db.UpdateCentro(centro);

        public async Task<bool> Delete(string id) => await db.DeleteCentro(id);
    }
}
