using GenteFit.Models;
using GenteFit.Models.Repositories.Collections;
using GenteFit.Models.Repositories.Interfaces;
using GenteFit.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GenteFit.Controllers.ControllersMongoDB
{
    /* Controlador DTO para la clase Administrador y el DAO AdministradorCollection. 
      Simplificamos al máximo la clase ya que la lógica irá en la API central. */
    public class AdministradorController : Controller
    {
        // Instanciamos la interfaz del Modelo MongoDB
        private IAdministrador db = new AdministradorCollection();

        public async Task<List<Administrador>> GetAllAdministradores() => await db.GetAllAdministradores();

        public async Task<Administrador> Details(string id) => await db.GetAdministradorById(id);

        public async Task<bool> Create(Administrador administrador) => await db.InsertAdministrador(administrador);

        public async Task<bool> Edit(Administrador administrador) => await db.UpdateAdministrador(administrador);
        
        public async Task<bool> Delete(string id) => await db.DeleteAdministrador(id);

        // Comprobamos si existen documentos
        public async Task<bool> IsEmpty() => await db.IsEmpty();
    }
}
