using Cervantes.Contracts;
using Cervantes.CORE;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Cervantes.Web.Controllers
{
    public class VulnController : Controller
    {

        IVulnManager vulnManager = null;
        IProjectManager projectManager = null;
        ITargetManager targetManager = null;
        IVulnCategoryManager vulnCategoryManager = null;
        IVulnNoteManager vulnNoteManager = null;
        IVulnAttachmentManager vulnAttachmentManager = null;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly ILogger<VulnController> _logger = null;

        public VulnController(IVulnManager vulnManager, IProjectManager projectManager, ILogger<VulnController> logger, ITargetManager targetManager,
            IVulnCategoryManager vulnCategoryManager, IVulnNoteManager vulnNoteManager, IVulnAttachmentManager vulnAttachmentManager, IHostingEnvironment _appEnvironment)
        {
            this.vulnManager = vulnManager;
            this.projectManager = projectManager;
            this.targetManager = targetManager;
            this.vulnCategoryManager = vulnCategoryManager;
            this.vulnAttachmentManager = vulnAttachmentManager;
            this.vulnNoteManager = vulnNoteManager;
            this._appEnvironment = _appEnvironment;
            _logger = logger;
        }

        // GET: VulnController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Categories()
        {
            try
            {
                var model = vulnCategoryManager.GetAll();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred loading Admin Vuln Categories. User: {0}", User.FindFirstValue(ClaimTypes.Name));
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


        public ActionResult CreateCategory()
        {
            return View();
        }

        // POST: VulnController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(IFormCollection collection)
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
        public ActionResult EditCategory(int id)
        {
            try
            {
                var model = vulnCategoryManager.GetById(id);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred loading Admin Delete Vuln Category. User: {0}, Vuln category: {1}", User.FindFirstValue(ClaimTypes.Name), id);
                return View();
            }
        }

        // POST: VulnController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(int id, VulnCategory model)
        {
            try
            {
                var result = vulnCategoryManager.GetById(id);
                result.Name = model.Name;
                result.Description = model.Description;
                vulnCategoryManager.Context.SaveChanges();
                _logger.LogInformation("User: {0} edited Vuln Category: {1}", User.FindFirstValue(ClaimTypes.Name), id);
                return RedirectToAction(nameof(Categories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred loading Admin Delete Vuln Category. User: {0}, Vuln category: {1}", User.FindFirstValue(ClaimTypes.Name), id);
                return View();
            }
        }

        // GET: VulnController/Delete/5
        public ActionResult DeleteCategory(int id)
        {

            try
            {
                var model = vulnCategoryManager.GetById(id);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred loading Admin Delete Vuln Category. User: {0}, Vuln category: {1}", User.FindFirstValue(ClaimTypes.Name), id);
                return View();
            }
        }

        // POST: VulnController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int id, IFormCollection collection)
        {
            try
            {
                var result = vulnCategoryManager.GetById(id);

                if (result != null)
                {
                    vulnCategoryManager.Remove(result);
                    vulnCategoryManager.Context.SaveChanges();
                    _logger.LogInformation("User: {0} deleted Delete Vuln Category: {1}", User.FindFirstValue(ClaimTypes.Name), id);
                    return RedirectToAction(nameof(Categories));
                }

                return RedirectToAction(nameof(Categories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred loading Admin Delete Vuln Category. User: {0}, Vuln category: {1}", User.FindFirstValue(ClaimTypes.Name), id);
                return View();
            }
        }
    }
}
