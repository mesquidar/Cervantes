using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Areas.Workspace.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Cervantes.Web.Areas.Workspace.Controllers
{
    [Area("Workspace")]
    public class TargetController : Controller
    {
        ITargetManager targetManager = null;
        ITargetServicesManager targetServicesManager = null;
        IProjectManager projectManager = null;

        /// <summary>
        /// Target Controller Constructor
        /// </summary>
        /// <param name="targetManager">TargetManager</param>
        /// <param name="targetServicesManager">TargetServiceManager</param>
        /// <param name="projectManager">ProjectManager</param>
        public TargetController(ITargetManager targetManager, ITargetServicesManager targetServicesManager, IProjectManager projectManager)
        {
            this.targetManager = targetManager;
            this.targetServicesManager = targetServicesManager;
            this.projectManager = projectManager;
        }

        /// <summary>
        /// Method return index Target
        /// </summary>
        /// <param name="project">project id</param>
        /// <returns></returns>
        public ActionResult Index(int project)
        {
            try
            {
                TargetViewModel model = new TargetViewModel
                {
                    Project = projectManager.GetById(project),
                    Target = targetManager.GetAll().Where(x => x.ProjectId == project)
                };
                return View(model);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Method retusn details of a Target
        /// </summary>
        /// <param name="project">Project Id</param>
        /// <param name="id">Target Id</param>
        /// <returns></returns>
        public ActionResult Details(int project, int id)
        {
            try
            {
                TargetDetailsViewModel model = new TargetDetailsViewModel
                {
                    Project = projectManager.GetById(project),
                    Target = targetManager.GetById(id),
                    TargetServices = targetServicesManager.GetAll().Where(x => x.TargetId == id)

                };
                return View(model);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Method retur creation form of Traget
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Method saves Target form
        /// </summary>
        /// <param name="project">Project Id</param>
        /// <param name="model">Target</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int project, Target model)
        {
            try
            {
                Target target = new Target
                {
                    Name = model.Name,
                    Type = model.Type,
                    Description = model.Description,
                    ProjectId = project,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                targetManager.Add(target);
                targetManager.Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Method return edit form of a target
        /// </summary>
        /// <param name="project">Project Id</param>
        /// <param name="id">Target Id</param>
        /// <returns></returns>
        public ActionResult Edit(int project, int id)
        {
            try
            {
                var model = targetManager.GetById(id);
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        /// <summary>
        /// Method saves edit form of a Target
        /// </summary>
        /// <param name="project">Project Id</param>
        /// <param name="model">Target</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int project, Target model)
        {
            try
            {
                var result = targetManager.GetById(model.Id);
                result.Name = model.Name;
                result.Type = model.Type;
                result.Description = model.Description;
                result.ProjectId = project;

                targetManager.Context.SaveChanges();
                TempData["edited"] = "edited";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Method return view to delete target
        /// </summary>
        /// <param name="project">Porject Id</param>
        /// <param name="id">Target Id</param>
        /// <returns></returns>
        public ActionResult Delete(int project, int id)
        {
            try
            {
                var model = targetManager.GetById(id);
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        /// <summary>
        /// Method confirm delete of a target
        /// </summary>
        /// <param name="project">Project Id</param>
        /// <param name="id">Target Id</param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int project, int id, IFormCollection collection)
        {
            try
            {
                var result = targetManager.GetById(id);
                if (result != null)
                {
                    targetManager.Remove(result);
                    targetManager.Context.SaveChanges();
                }

                TempData["deleted"] = "deleted";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        /// <summary>
        /// Method return a Traget Service Details
        /// </summary>
        /// <param name="project">Project Id</param>
        /// <param name="id">Target Service Id</param>
        /// <returns></returns>
        public ActionResult Service(int project, int id)
        {
            try
            {
                TargetServiceViewModel model = new TargetServiceViewModel
                {
                    Project = projectManager.GetById(project),
                    Target = targetManager.GetById(id),
                    TargetService = targetServicesManager.GetById(id),

                };
                return View(model);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult AddService(int project, int id)
        {
            try
            {
                TargetServiceViewModel model = new TargetServiceViewModel
                {
                    Project = projectManager.GetById(project),
                    Target = targetManager.GetById(id),

                };
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddService(int project, TargetServiceViewModel model)
        {
            try
            {
                TargetServices result = new TargetServices
                {
                    Name = model.TargetService.Name,
                    Version = model.TargetService.Version,
                    Port = model.TargetService.Port,
                    Description = model.TargetService.Description,
                    Note = model.TargetService.Note,
                    TargetId = model.Target.Id,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                targetServicesManager.Add(result);
                targetServicesManager.Context.SaveChanges();
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult EditService(int project, int id, int target)
        {
            try
            {
                TargetServiceViewModel model = new TargetServiceViewModel
                {
                    Project = projectManager.GetById(project),
                    Target = targetManager.GetById(target),
                    TargetService = targetServicesManager.GetById(id)

                };
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditService(IFormCollection collection)
        {
            return View();
        }

        public ActionResult DeleteService(int project, int id)
        {
            try
            {
                var model = targetServicesManager.GetById(id);
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteService(int project, int id, IFormCollection collection)
        {
            try
            {
                var result = targetServicesManager.GetById(id);
                if (result != null)
                {
                    targetServicesManager.Remove(result);
                    targetServicesManager.Context.SaveChanges();
                }

                TempData["deleted"] = "deleted";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }




    }
}
