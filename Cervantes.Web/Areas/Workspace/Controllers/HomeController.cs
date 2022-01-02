﻿using Cervantes.Contracts;
using Cervantes.Web.Areas.Workspace.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Cervantes.Web.Areas.Workspace.Controllers
{
    [Area("Workspace")]
    public class HomeController : Controller
    {


        IProjectManager projectManager = null;
        IClientManager clientManager = null;
        IVulnManager vulnManager = null;
        ITaskManager taskManager = null;
        IDocumentManager documentManager = null;
        IProjectUserManager projectUserManager = null;
        ITargetManager targetManager = null;
        IProjectNoteManager projectNoteManager = null;
        IProjectAttachmentManager projectAttachmentManager = null;

        public HomeController(IProjectManager projectManager, IClientManager clientManager, IVulnManager vulnManager, ITaskManager taskManager, IDocumentManager documentManager,
            IProjectUserManager projectUserManager, ITargetManager targetManager, IProjectNoteManager projectNoteManager, IProjectAttachmentManager projectAttachmentManager)
        {
            this.projectManager = projectManager;
            this.clientManager = clientManager;
            this.vulnManager = vulnManager;
            this.taskManager = taskManager;
            this.documentManager = documentManager;
            this.projectUserManager = projectUserManager;
            this.targetManager = targetManager;
            this.projectNoteManager = projectNoteManager;
            this.projectAttachmentManager = projectAttachmentManager;
        }

        public ActionResult Index(int project)
        {
            try
            {
                DashboardViewModel model = new DashboardViewModel
                {
                    Project = projectManager.GetById(project),
                    Members = projectUserManager.GetAll().Where(x => x.ProjectId == project),
                    Vulns = vulnManager.GetAll().Where(x => x.ProjectId == project).OrderByDescending(x => x.CreatedDate).Take(10),
                    Tasks = taskManager.GetAll().Where(x => x.AsignedUserId == User.FindFirstValue(ClaimTypes.NameIdentifier) && x.ProjectId == project),
                    Targets = targetManager.GetAll().Where(x => x.ProjectId == project),
                    Notes = projectNoteManager.GetAll().Where(x => x.ProjectId == project),
                    VulnNumber = vulnManager.GetAll().Where(x => x.ProjectId == project).Count(),
                    TasksNumber = taskManager.GetAll().Where(x => x.ProjectId == project).Count(),
                    TargetsNumber = targetManager.GetAll().Where(x => x.ProjectId == project).Count(),
                    MembersNumber = projectUserManager.GetAll().Where(x => x.ProjectId == project).Count(),
                    NotesNumber = projectNoteManager.GetAll().Where(x => x.ProjectId == project).Count(),
                    AttachmentsNumber = projectAttachmentManager.GetAll().Where(x => x.ProjectId == project).Count(),
                    VulnInfo = vulnManager.GetAll().Where(x => x.ProjectId == project && x.Risk == CORE.VulnRisk.Info).Count(),
                    VulnLow = vulnManager.GetAll().Where(x => x.ProjectId == project && x.Risk == CORE.VulnRisk.Low).Count(),
                    VulnMedium = vulnManager.GetAll().Where(x => x.ProjectId == project && x.Risk == CORE.VulnRisk.Medium).Count(),
                    VulnHigh = vulnManager.GetAll().Where(x => x.ProjectId == project && x.Risk == CORE.VulnRisk.High).Count(),
                    VulnCritical = vulnManager.GetAll().Where(x => x.ProjectId == project && x.Risk == CORE.VulnRisk.Critical).Count()
                };
                return View(model);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
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

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
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

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
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
