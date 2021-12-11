using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cervantes.Web.Areas.Workspace.Controllers
{
    public class VulnController : Controller
    {
        // GET: VulnController
        public ActionResult Index()
        {
            return View();
        }

        // GET: VulnController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VulnController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VulnController/Create
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

        // GET: VulnController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VulnController/Edit/5
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

        // GET: VulnController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VulnController/Delete/5
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
