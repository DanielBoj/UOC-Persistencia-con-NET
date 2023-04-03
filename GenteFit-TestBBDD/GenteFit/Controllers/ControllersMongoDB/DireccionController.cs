using Microsoft.AspNetCore.Mvc;

namespace GenteFit.Controllers.ControllersMongoDB
{
    public class DireccionController : Controller
    {
        // GET: DireccionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DireccionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DireccionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DireccionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DireccionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DireccionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DireccionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DireccionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
