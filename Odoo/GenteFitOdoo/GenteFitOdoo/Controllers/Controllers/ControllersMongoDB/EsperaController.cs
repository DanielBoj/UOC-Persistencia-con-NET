using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenteFitOdoo.Controllers.Controllers.ControllersMongoDB
{
    /* Controlador DTO para la clase Espera y el DAO EsperaCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class EsperaController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IEspera db = new EsperaCollection();

        public async Task<List<Espera>> GetAllEsperas() => await db.GetAllEsperas();

        public async Task<Espera> Details(string id) => await db.GetEsperaById(id);

        public async Task<bool> Create(Espera espera) => await db.InsertEspera(espera);

        public async Task<bool> Edit(Espera espera) => await db.UpdateEspera(espera);

        public async Task<bool> Delete(string id) => await db.DeleteEspera(id);
    }
}
