using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Cervantes.Web.Controllers
{
    public class ProjectController : Controller
    {
        IProjectManager projectManager = null;
        IClientManager clientManager = null;

        /// <summary>
        /// ProjectController Constructor
        /// </summary>
        /// <param name="projectManager">ProjectManager</param>
        /// <param name="clientManager">ClientManager</param>
        public ProjectController(IProjectManager projectManager, IClientManager clientManager)
        {
            this.projectManager = projectManager;
            this.clientManager = clientManager;
        }

        /// <summary>
        /// Method Index show all projects
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            try
            {

                var model = projectManager.GetAll().Select(e => new ProjectViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Client = e.Client,
                    ProjectType = e.ProjectType,
                    Status = e.Status,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    ClientId = e.ClientId,


                });

                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    TempData["empty"] = "No clients introduced";
                    return View();
                }


            }
            catch (Exception ex)
            {
                return View();
            }
        }

        /// <summary>
        /// Methos show project details
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            return View();
        }

        /// <summary>
        /// Method sheo create form
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var result = clientManager.GetAll().Select(e => new ClientViewModel
            {
                Id = e.Id,
                Name = e.Name,
            }).ToList();

            var li = new List<SelectListItem>();

            foreach (var item in result)
            {
                li.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }

            var model = new ProjectViewModel
            {
                ItemList = li
            };

            return View(model);
        }

        /// <summary>
        /// Method save reate form
        /// </summary>
        /// <param name="model">ProjectViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Create(ProjectViewModel model)
        {
            try
            {
                var item = model.ItemList;

                Project project = new Project
                {
                    Name = model.Name,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ClientId = model.ClientId,
                    ProjectType = model.ProjectType,
                    Status = model.Status,
                    Template = model.Template,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)

                };
                projectManager.Add(project);
                projectManager.Context.SaveChanges();
                TempData["created"] = "created";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Method show edit form
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            return View();
        }

        /// <summary>
        /// Method save edit form
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
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

        // GET: ProjectController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectController/Delete/5
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

        /// <summary>
        /// Method show all template projects
        /// </summary>
        /// <returns></returns>
        public ActionResult Template()
        {
            try
            {

                var model = projectManager.GetAll().Where(x => x.Template == true).Select(e => new ProjectViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Client = e.Client,
                    ProjectType = e.ProjectType,
                    Status = e.Status,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    ClientId = e.ClientId,


                });

                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    TempData["empty"] = "No clients introduced";
                    return View();
                }


            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
