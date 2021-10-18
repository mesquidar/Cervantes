using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cervantes.Web.Controllers
{
    public class WorkspaceController : Controller
    {
        // GET: WorkspaceController
        public ActionResult Index()
        {
            return View();
        }

        // GET: WorkspaceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WorkspaceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkspaceController/Create
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

        // GET: WorkspaceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WorkspaceController/Edit/5
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

        // GET: WorkspaceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WorkspaceController/Delete/5
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
