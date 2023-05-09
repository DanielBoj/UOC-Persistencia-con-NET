using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GenteFit.Models.Enums;
using GenteFit.Models.Collections;
using System.Dynamic;

namespace GenteFitOdoo.Controllers.Controllers.ControllersMongoDB
{
    /* Controlador DTO para la clase Horario y el DAO HorarioCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class HorarioController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IHorario db = new HorarioCollection();

        public async Task<List<Horario>> GetAllHorarios() => await db.GetAllHorarios();

        public async Task<Horario> Details(string id) => await db.GetHorarioById(id);

        public async Task<bool> Create(Horario horario) => await db.InsertHorario(horario);

        public async Task<bool> Edit(Horario horario) => await db.UpdateHorario(horario);

        public async Task<bool> Delete(string id) => await db.DeleteHorario(id);
    }
}
