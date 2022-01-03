using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Areas.Workspace.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Cervantes.Web.Areas.Workspace.Controllers
{
    [Area("Workspace")]
    public class TaskController : Controller
    {
        IProjectManager projectManager = null;
        ITaskManager taskManager = null;
        ITargetManager targetManager = null;

        public TaskController(ITaskManager taskManager, IProjectManager projectManager, ITargetManager targetManager)
        {
            this.projectManager = projectManager;
            this.taskManager = taskManager;
            this.targetManager = targetManager;
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

        // GET: TaskController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaskController/Edit/5
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
    }
}
