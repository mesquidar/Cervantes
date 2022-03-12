using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;
using System;
using System.IO;
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
        IReportManager reportManager = null;
        public ReportController(IProjectManager projectManager, IClientManager clientManager, IOrganizationManager organizationManager, IProjectUserManager projectUserManager, IProjectNoteManager projectNoteManager,
           IProjectAttachmentManager projectAttachmentManager, ITargetManager targetManager, ITaskManager taskManager, IUserManager userManager, IVulnManager vulnManager, IHostingEnvironment _appEnvironment,
           ILogger<ReportController> logger, IReportManager reportManager)
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
            this.reportManager = reportManager;
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
                    Reports = reportManager.GetAll().Where(x => x.ProjectId == id),
                };


                return new ViewAsPdf(model, ViewData);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred generating report for Project: {0}. User: {1}", id, User.FindFirstValue(ClaimTypes.Name));
                return View();
            }
        }

        [HttpPost]
        public IActionResult Generate(IFormCollection form)
        {
            try
            {


                ReportViewModel model = new ReportViewModel
                {
                    Organization = organizationManager.GetById(1),
                    Project = projectManager.GetById(Int32.Parse(form["project"])),
                    Vulns = vulnManager.GetAll().Where(x => x.ProjectId == Int32.Parse(form["project"])),
                    Targets = targetManager.GetAll().Where(x => x.ProjectId == Int32.Parse(form["project"])),
                    Users = projectUserManager.GetAll().Where(x => x.ProjectId == Int32.Parse(form["project"])),
                    Reports = reportManager.GetAll().Where(x => x.ProjectId == Int32.Parse(form["project"])),
                };


                //var report = new ViewAsPdf(model, ViewData);
                var uploads = Path.Combine(_appEnvironment.WebRootPath, "Attachments/Reports/" + form["project"] + "/");
                var uniqueName = Guid.NewGuid().ToString() + "_" + form["reportName"] + ".pdf";

                if (Directory.Exists(uploads))
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, uniqueName), FileMode.Create, FileAccess.Write))
                    {
                        var pdfResult = new ViewAsPdf(model)
                        {
                            FileName = uniqueName,
                            CustomSwitches =
            "--footer-center \"  " + "Page: [page]/[toPage]\"" +
          " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
                        };
                        byte[] pdfData = pdfResult.BuildFile(ControllerContext).Result;
                        fileStream.Write(pdfData, 0, pdfData.Length);
                    }
                }
                else
                {
                    Directory.CreateDirectory(uploads);

                    using (var fileStream = new FileStream(Path.Combine(uploads, uniqueName), FileMode.Create, FileAccess.Write))
                    {
                        var pdfResult = new ViewAsPdf(model)
                        {
                            FileName = uniqueName,
                            CustomSwitches =
            "--footer-center \"  " + "Page: [page]/[toPage]\"" +
          " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
                        };
                        byte[] pdfData = pdfResult.BuildFile(ControllerContext).Result;
                        fileStream.Write(pdfData, 0, pdfData.Length);
                    }
                }

                Report rep = new Report
                {
                    Name = form["reportName"],
                    ProjectId = Int32.Parse(form["project"]),
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    CreatedDate = DateTime.Now,
                    Description = form["description"],
                    Version = form["version"],
                    FilePath = "Attachments/Reports/" + form["project"] + "/" + uniqueName
                };

                reportManager.Add(rep);
                reportManager.Context.SaveChanges();


                return RedirectToAction("Details", "Project", new { id = Int32.Parse(form["project"]) });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred generating report for Project: {0}. User: {1}", Int32.Parse(form["project"]), User.FindFirstValue(ClaimTypes.Name));
                return View();
            }
        }

        public ActionResult Download(int id)
        {

            var report = reportManager.GetById(id);

            string filePath = Path.Combine(_appEnvironment.WebRootPath, report.FilePath);
            string fileName = report.Project.Name + "_" + report.Name + "_v" + report.Version + ".pdf";

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/PDF", fileName);

        }
    }
}
