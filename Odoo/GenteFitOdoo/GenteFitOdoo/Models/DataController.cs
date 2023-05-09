using GenteFitOdoo.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GenteFit.Models
{
    public class DataController : Controller
    {
        public Api Controlador;
        
        public DataController () { 
            this.Controlador = new Api();
        }

        public DataController (Api controlador) {
            Controlador = controlador;
        }
    }
}
