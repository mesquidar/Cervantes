using Cervantes.Contracts;
using Cervantes.Web.Areas.Workspace.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Cervantes.Web.Areas.Workspace.Controllers
{
    [Area("Workspace")]
    public class VulnController : Controller
    {
        IVulnManager vulnManager = null;
        IProjectManager projectManager = null;

        public VulnController(IVulnManager vulnManager, IProjectManager projectManager)
        {
            this.vulnManager = vulnManager;
            this.projectManager = projectManager;
        }
        // GET: VulnController
        public ActionResult Index(int project)
        {
            try
            {
                VulnViewModel model = new VulnViewModel
                {
                    Project = projectManager.GetById(project),
                    Vulns = vulnManager.GetAll().Where(x => x.ProjectId == project)
                };
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }

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
