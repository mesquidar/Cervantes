using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Cervantes.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
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
            IProjectAttachmentManager projectAttachmentManager, ITargetManager targetManager, ITaskManager taskManager, IUserManager userManager, IVulnManager vulnManager, IHostingEnvironment _appEnvironment)
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
            this._appEnvironment = _appEnvironment;
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
        public ActionResult Templates()
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
        /// Method retrieve project template by id
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <returns></returns>
        public ActionResult Template(int id)
        {
            try
            {
                var template = projectManager.GetById(id);
                if (template.Template == true)
                {

                }
                else
                {
                    TempData["errorTemplate"] = "added";
                    return View("Templates");
                }
              return View();

            }
            catch (Exception ex)
            {
                return View();
            }
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

        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        public IActionResult AddTarget(IFormCollection form)
        {

            if (form != null)
            {

                Target target = new Target
                { 
                   Name = form["name"],
                   Description = form["description"],
                   ProjectId = Int32.Parse(form["project"]),
                   UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                   Type = (TargetType)Enum.ToObject(typeof(TargetType), Int32.Parse(form["TargetType"])),

                };

                targetManager.Add(target);
                targetManager.Context.SaveChanges();
                TempData["addedTarget"] = "added";
                return RedirectToAction("Details", "Project", new { id = Int32.Parse(form["project"]) });
            }
            else
            {
                return RedirectToAction("Details", "Project", new { id = Int32.Parse(form["project"]) });
            }

        }

        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        public IActionResult DeleteTarget(int target, int project)
        {

            if (target != 0)
            {
                var result = targetManager.GetById(target);

                targetManager.Remove(result);
                targetManager.Context.SaveChanges();
                TempData["deletedTarget"] = "deleted";
                return RedirectToAction("Details", "Project", new { id = project });
            }
            else
            {
                return RedirectToAction("Details", "Project", new { id = project });
            }

        }

        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        public IActionResult AddNote(IFormCollection form)
        {

            if (form != null)
            {

                ProjectNote note = new ProjectNote
                {
                    Name = form["noteName"],
                    Description = form["noteDescription"],
                    ProjectId = Int32.Parse(form["project"]),
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Visibility = (Visibility)Enum.ToObject(typeof(Visibility), Int32.Parse(form["Visibility"])),

                };

                projectNoteManager.Add(note);
                projectNoteManager.Context.SaveChanges();
                TempData["addedNote"] = "added";
                return RedirectToAction("Details", "Project", new { id = Int32.Parse(form["project"]) });
            }
            else
            {
                return RedirectToAction("Details", "Project", new { id = Int32.Parse(form["project"]) });
            }

        }

        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        public IActionResult DeleteNote(int id, int project)
        {

            if (id != 0)
            {
                var result = projectNoteManager.GetById(id);

                projectNoteManager.Remove(result);
                projectNoteManager.Context.SaveChanges();
                TempData["deletedNote"] = "deleted";
                return RedirectToAction("Details", "Project", new { id = project });
            }
            else
            {
                return RedirectToAction("Details", "Project", new { id = project });
            }

        }

        

        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        public IActionResult AddAttachment(IFormCollection form, IFormFile upload)
        {

            if (form != null && upload != null)
            {
                var file = Request.Form.Files["upload"];
                var uploads = Path.Combine(_appEnvironment.WebRootPath, "Attachments/Project/"+form["project"]+"/");
                var uniqueName = Guid.NewGuid().ToString() + "_" + file.FileName;

                if (Directory.Exists(uploads))
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, uniqueName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                    }
                }
                else
                {
                    Directory.CreateDirectory(uploads);
                    
                    using (var fileStream = new FileStream(Path.Combine(uploads, uniqueName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                    }
                }

                

                ProjectAttachment note = new ProjectAttachment
                {
                    Name = form["attachmentName"],
                    ProjectId = Int32.Parse(form["project"]),
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    FilePath = "/Attachments/Project/" + form["project"]+"/"+ uniqueName,

                };

                projectAttachmentManager.Add(note);
                projectAttachmentManager.Context.SaveChanges();
                TempData["addedAttachment"] = "added";
                return RedirectToAction("Details", "Project", new { id = Int32.Parse(form["project"]) });
            }
            else
            {
                TempData["errorAttachment"] = "added";
                return RedirectToAction("Details", "Project", new { id = Int32.Parse(form["project"]) });
            }

        }

        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        public IActionResult DeleteAttachment(int id, int project)
        {

            if (id != 0)
            {
                var result = projectAttachmentManager.GetById(id);

                var pathFile = _appEnvironment.WebRootPath + result.FilePath;
                if (System.IO.File.Exists(pathFile))
                {
                    System.IO.File.Delete(pathFile);
                }

                projectAttachmentManager.Remove(result);
                projectAttachmentManager.Context.SaveChanges();
                TempData["deletedAttachment"] = "deleted";
                return RedirectToAction("Details", "Project", new { id = project });
            }
            else
            {
                return RedirectToAction("Details", "Project", new { id = project });
            }

        }

    }
}
