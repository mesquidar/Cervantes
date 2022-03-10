using Cervantes.Contracts;
using Cervantes.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;

namespace Cervantes.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger = null;
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
        IOrganizationManager organizationManager = null;
        public ReportController(IProjectManager projectManager, IClientManager clientManager, IOrganizationManager organizationManager, IProjectUserManager projectUserManager, IProjectNoteManager projectNoteManager,
           IProjectAttachmentManager projectAttachmentManager, ITargetManager targetManager, ITaskManager taskManager, IUserManager userManager, IVulnManager vulnManager, IHostingEnvironment _appEnvironment,
           ILogger<ReportController> logger)
        {
            this.projectManager = projectManager;
            this.clientManager = clientManager;
            this.organizationManager = organizationManager;
            this.projectUserManager = projectUserManager;
            this.projectNoteManager = projectNoteManager;
            this.projectAttachmentManager = projectAttachmentManager;
            this.targetManager = targetManager;
            this.taskManager = taskManager;
            this.userManager = userManager;
            this.vulnManager = vulnManager;
            this._appEnvironment = _appEnvironment;
            _logger = logger;
        }

        public IActionResult Index(int id)
        {
            try
            {
                ReportViewModel model = new ReportViewModel
                {
                    Organization = organizationManager.GetById(1),
                    Project = projectManager.GetById(id),
                    Vulns = vulnManager.GetAll().Where(x => x.ProjectId == id),
                    Targets = targetManager.GetAll().Where(x => x.ProjectId == id),
                    Users = projectUserManager.GetAll().Where(x => x.ProjectId == id),

                };
                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred generating report for Project: {0}. User: {1}", id, User.FindFirstValue(ClaimTypes.Name));
                return View();
            }
        }
    }
}
