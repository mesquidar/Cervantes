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
        IProjectUserManager projectUserManager = null;
        IProjectNoteManager projectNoteManager = null;
        IProjectAttachmentManager projectAttachmentManager = null;
        ITargetManager targetManager = null;
        ITaskManager taskManager = null;
        IUserManager userManager = null;
        IVulnManager vulnManager = null;

        /// <summary>
        /// ProjectController Constructor
        /// </summary>
        /// <param name="projectManager">ProjectManager</param>
        /// <param name="clientManager">ClientManager</param>
        public ProjectController(IProjectManager projectManager, IClientManager clientManager, IProjectUserManager projectUserManager, IProjectNoteManager projectNoteManager, 
            IProjectAttachmentManager projectAttachmentManager, ITargetManager targetManager, ITaskManager taskManager, IUserManager userManager, IVulnManager vulnManager)
        {
            this.projectManager = projectManager;
            this.clientManager = clientManager;
            this.projectUserManager = projectUserManager;
            this.projectNoteManager = projectNoteManager;
            this.projectAttachmentManager = projectAttachmentManager;   
            this.targetManager = targetManager;
            this.taskManager = taskManager;
            this.userManager = userManager;
            this.vulnManager = vulnManager;
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
            var project = projectManager.GetById(id);
            if (project != null)
            {

                var result = userManager.GetAll().Select(e => new ApplicationUser
                {
                    Id = e.Id,
                    FullName = e.FullName,
                }).ToList();

                var users = new List<SelectListItem>();

                foreach (var item in result)
                {
                    users.Add(new SelectListItem { Text = item.FullName, Value = item.Id.ToString() });
                }

                ProjectDetailsViewModel model = new ProjectDetailsViewModel
                {
                    Project = project,
                    ProjectUsers = projectUserManager.GetAll().Where( x => x.ProjectId == id),
                    ProjectNotes = projectNoteManager.GetAll().Where(x => x.ProjectId == id),
                    ProjectAttachments = projectAttachmentManager.GetAll().Where(x => x.ProjectId == id),
                    Targets = targetManager.GetAll().Where(x => x.ProjectId == id),
                    Tasks = taskManager.GetAll().Where(x => x.ProjectId == id),
                    Users = users,
                    Vulns = vulnManager.GetAll().Where(x => x.ProjectId == id),

                };
                return View(model);
            }

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

        /// <summary>
        /// Method return manage memebers page
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperUser")]
        public IActionResult Members(int id)
        {
            return View();
            
        }

        /// <summary>
        /// Method Add user to project
        /// </summary>
        /// <param name="project">Project Id</param>
        /// <param name="user">User Id</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        public IActionResult AddMember(int project, string users)
        {
            if (project != null && users != null)
            {

                var result = projectUserManager.GetAll().Where(x => x.UserId == users && x.ProjectId == project);

                if(result.FirstOrDefault() == null)
                {
                    ProjectUser user = new ProjectUser
                    {
                        ProjectId = project,
                        UserId = users
                    };
                    projectUserManager.Add(user);
                    projectUserManager.Context.SaveChanges();
                    TempData["addedMember"] = "added";
                    return RedirectToAction("Details", "Project", new { id = project });
                }
                else
                {
                    TempData["existsMember"] = "exists";
                    return RedirectToAction("Details", "Project", new { id = project });
                }
                
            }
            else
            {
                return RedirectToAction("Details", "Project", new { id = project });
            }
            
        }

        /// <summary>
        /// Method delete user from project
        /// </summary>
        /// <param name="project">Project Id</param>
        /// <param name="user">User Id</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        public IActionResult DeleteMember(int project, string user)
        {
            var result = projectUserManager.GetAll().Where(x => x.UserId == user && x.ProjectId == project);
            if (result.FirstOrDefault() != null)
            {
                projectUserManager.Remove(result.FirstOrDefault());
                projectUserManager.Context.SaveChanges();
                TempData["deletedMember"] = "deleted";
                return RedirectToAction("Details", "Project", new { id = project });
            }

            return RedirectToAction("Details", "Project", new { id = project });
        }


    }
}
