using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cervantes.Web.Areas.Workspace.Controllers
{
    public class TargetController : Controller
    {
        // GET: TargetController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TargetController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TargetController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TargetController/Create
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

        // GET: TargetController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TargetController/Edit/5
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

        // GET: TargetController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TargetController/Delete/5
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
