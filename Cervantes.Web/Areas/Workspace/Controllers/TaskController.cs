using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Areas.Workspace.Models;
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

namespace Cervantes.Web.Areas.Workspace.Controllers
{
    [Area("Workspace")]
    public class TaskController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;

        IProjectManager projectManager = null;
        IProjectUserManager projectUserManager = null;
        ITaskManager taskManager = null;
        ITaskNoteManager taskNoteManager = null;
        ITaskAttachmentManager taskAttachmentManager = null;
        ITargetManager targetManager = null;

        public TaskController(IHostingEnvironment _appEnvironment, ITaskManager taskManager, IProjectManager projectManager, ITargetManager targetManager, ITaskNoteManager taskNoteManager, ITaskAttachmentManager taskAttachmentManager,
            IProjectUserManager projectUserManager)
        {
            this.projectManager = projectManager;
            this.projectUserManager = projectUserManager;
            this.taskManager = taskManager;
            this.targetManager = targetManager;
            this.taskNoteManager = taskNoteManager;
            this.taskAttachmentManager = taskAttachmentManager;
            this._appEnvironment = _appEnvironment;
        }

        public ActionResult Index(int project)
        {
            try
            {
                TaskViewModel model = new TaskViewModel
                {
                    Project = projectManager.GetById(project),
                    Tasks = taskManager.GetAll().Where(x => x.AsignedUserId == User.FindFirstValue(ClaimTypes.NameIdentifier) && x.ProjectId == project),
                };
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }

        }

        public ActionResult Project(int project)
        {
            try
            {
                TaskViewModel model = new TaskViewModel
                {
                    Project = projectManager.GetById(project),
                    Tasks = taskManager.GetAll().Where(x => x.ProjectId == project),
                };
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }

        }

        // GET: TaskController/Details/5
        public ActionResult Details(int project, int id)
        {
            try
            {
                TaskDetailsViewModel model = new TaskDetailsViewModel
                {
                    Project = projectManager.GetById(project),
                    Task = taskManager.GetById(id),
                    Notes = taskNoteManager.GetAll().Where(x => x.TaskId == id),
                    Attachments = taskAttachmentManager.GetAll().Where(x => x.TaskId == id)

                };
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }

        }

        // GET: TaskController/Create
        public ActionResult Create(int project)
        {
            try
            {

                var result = targetManager.GetAll().Where(x => x.ProjectId == project).Select(e => new TaskCreateViewModel
                {
                    TargetId = e.Id,
                    TargetName = e.Name,
                }).ToList();

                var li = new List<SelectListItem>();

                foreach (var item in result)
                {
                    li.Add(new SelectListItem { Text = item.TargetName, Value = item.TargetId.ToString() });
                }

                var model = new TaskCreateViewModel
                {
                    Project = projectManager.GetById(project),
                    TargetList = li
                };

                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }

        }

        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int project, TaskCreateViewModel model)
        {
            try
            {
                Task task = new Task
                {
                    Name = model.Name,
                    Description = model.Description,
                    ProjectId = project,
                    TargetId = model.TargetId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Status = model.Status,
                    AsignedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    CreatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                };

                taskManager.Add(task);
                taskManager.Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult CreateProject(int project)
        {
            try
            {

                var targets = targetManager.GetAll().Where(x => x.ProjectId == project).Select(e => new TaskCreateViewModel
                {
                    TargetId = e.Id,
                    TargetName = e.Name,
                }).ToList();

                var li = new List<SelectListItem>();

                foreach (var item in targets)
                {
                    li.Add(new SelectListItem { Text = item.TargetName, Value = item.TargetId.ToString() });
                }

                var users = projectUserManager.GetAll().Where(x => x.ProjectId == project);
                var li2 = new List<SelectListItem>();
                foreach (var item in users)
                {
                    li2.Add(new SelectListItem { Text = item.User.FullName, Value = item.User.Id.ToString() });
                }


                var model = new TaskCreateViewModel
                {
                    Project = projectManager.GetById(project),
                    TargetList = li,
                    UsersList = li2
                };

                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }

        }

        // POST: TaskController/Create
        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(int project, TaskCreateViewModel model)
        {
            try
            {
                Task task = new Task
                {
                    Name = model.Name,
                    Description = model.Description,
                    ProjectId = project,
                    TargetId = model.TargetId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Status = model.Status,
                    AsignedUserId = model.AsignedUserId,
                    CreatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                };

                taskManager.Add(task);
                taskManager.Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: TaskController/Edit/5
        public ActionResult Edit(int project, int id)
        {
            try
            {
                var result = taskManager.GetById(id);


                var target = targetManager.GetAll().Where(x => x.ProjectId == project).Select(e => new TaskCreateViewModel
                {
                    TargetId = e.Id,
                    TargetName = e.Name,
                }).ToList();

                var li = new List<SelectListItem>();

                foreach (var item in target)
                {
                    li.Add(new SelectListItem { Text = item.TargetName, Value = item.TargetId.ToString() });
                }

                TaskCreateViewModel model = new TaskCreateViewModel
                {
                    Project = projectManager.GetById(project),
                    Id = id,
                    Name = result.Name,
                    Description = result.Description,
                    TargetId = result.TargetId,
                    AsignedUserId = result.AsignedUserId,
                    CreatedUserId = result.CreatedUserId,
                    StartDate = result.StartDate,
                    EndDate = result.EndDate,
                    Status = result.Status,
                    TargetList = li,
                    ProjectId = project,

                };

                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int project, TaskCreateViewModel model, int id)
        {
            try
            {
                var result = taskManager.GetById(id);
                result.Name = model.Name;
                result.Description = model.Description;
                result.TargetId = model.TargetId;
                result.EndDate = model.EndDate;
                result.Status = model.Status;
                result.StartDate = model.StartDate;

                taskManager.Context.SaveChanges();
                TempData["edited"] = "edited";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditProject(int project, int id)
        {
            try
            {
                var result = taskManager.GetById(id);


                var targets = targetManager.GetAll().Where(x => x.ProjectId == project).Select(e => new TaskCreateViewModel
                {
                    TargetId = e.Id,
                    TargetName = e.Name,
                }).ToList();

                var li = new List<SelectListItem>();

                foreach (var item in targets)
                {
                    li.Add(new SelectListItem { Text = item.TargetName, Value = item.TargetId.ToString() });
                }

                var users = projectUserManager.GetAll().Where(x => x.ProjectId == project);
                var li2 = new List<SelectListItem>();
                foreach (var item in users)
                {
                    li2.Add(new SelectListItem { Text = item.User.FullName, Value = item.User.Id.ToString() });
                }


                TaskCreateViewModel model = new TaskCreateViewModel
                {
                    Project = projectManager.GetById(project),
                    Id = id,
                    Name = result.Name,
                    Description = result.Description,
                    TargetId = result.TargetId,
                    AsignedUserId = result.AsignedUserId,
                    StartDate = result.StartDate,
                    EndDate = result.EndDate,
                    Status = result.Status,
                    TargetList = li,
                    ProjectId = project,
                    UsersList = li2,

                };

                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(int project, TaskCreateViewModel model, int id)
        {
            try
            {
                var result = taskManager.GetById(id);
                result.Name = model.Name;
                result.Description = model.Description;
                result.TargetId = model.TargetId;
                result.EndDate = model.EndDate;
                result.Status = model.Status;
                result.StartDate = model.StartDate;
                result.AsignedUserId = model.AsignedUserId;

                taskManager.Context.SaveChanges();
                TempData["edited"] = "edited";
                return RedirectToAction("Project");
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskController/Delete/5
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

        [HttpPost]
        public IActionResult AddNote(int project, IFormCollection form)
        {

            if (form != null)
            {

                TaskNote note = new TaskNote
                {
                    Name = form["noteName"],
                    Description = form["noteDescription"],
                    TaskId = Int32.Parse(form["task"]),
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                };

                taskNoteManager.Add(note);
                taskNoteManager.Context.SaveChanges();
                TempData["addedNote"] = "added";
                return RedirectToAction("Details", "Task", new { project = project, id = Int32.Parse(form["task"]) });
            }
            else
            {
                return RedirectToAction("Details", "Project", new { project = project, id = Int32.Parse(form["task"]) });
            }

        }

        [HttpPost]
        public IActionResult DeleteNote(int task, int project, int id)
        {

            if (id != 0)
            {
                var result = taskNoteManager.GetById(id);

                taskNoteManager.Remove(result);
                taskNoteManager.Context.SaveChanges();
                TempData["deletedNote"] = "deleted";
                return RedirectToAction("Details", "Task", new { project = project, id = task });
            }
            else
            {
                return RedirectToAction("Details", "Task", new { project = project, id = task });
            }

        }



        [HttpPost]
        public IActionResult AddAttachment(int project, int task, IFormCollection form, IFormFile upload)
        {

            if (form != null && upload != null)
            {
                var file = Request.Form.Files["upload"];
                var uploads = Path.Combine(_appEnvironment.WebRootPath, "Attachments/Task/" + form["task"] + "/");
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



                TaskAttachment note = new TaskAttachment
                {
                    Name = form["attachmentName"],
                    TaskId = Int32.Parse(form["task"]),
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    FilePath = "/Attachments/Task/" + form["task"] + "/" + uniqueName,

                };

                taskAttachmentManager.Add(note);
                taskAttachmentManager.Context.SaveChanges();
                TempData["addedAttachment"] = "added";
                return RedirectToAction("Details", "Task", new { project = project, id = Int32.Parse(form["task"]) });
            }
            else
            {
                TempData["errorAttachment"] = "added";
                return RedirectToAction("Details", "Task", new { project = project, id = Int32.Parse(form["task"]) });
            }

        }


        [HttpPost]
        public IActionResult DeleteAttachment(int id, int project, int task)
        {

            if (id != 0)
            {
                var result = taskAttachmentManager.GetById(id);

                var pathFile = _appEnvironment.WebRootPath + result.FilePath;
                if (System.IO.File.Exists(pathFile))
                {
                    System.IO.File.Delete(pathFile);
                }

                taskAttachmentManager.Remove(result);
                taskAttachmentManager.Context.SaveChanges();
                TempData["deletedAttachment"] = "deleted";
                return RedirectToAction("Details", "Task", new { project = project, id = task });
            }
            else
            {
                return RedirectToAction("Details", "Task", new { project = project, id = task });
            }

        }

    }
}
