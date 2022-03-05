using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Areas.Workspace.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Cervantes.Web.Areas.Workspace.Controllers
{
    [Area("Workspace")]
    public class VulnController : Controller
    {
        IVulnManager vulnManager = null;
        IProjectManager projectManager = null;
        ITargetManager targetManager = null;
        IVulnCategoryManager vulnCategoryManager = null;
        IVulnNoteManager vulnNoteManager = null;
        IVulnAttachmentManager vulnAttachmentManager = null;
        private readonly ILogger<VulnController> _logger = null;

        public VulnController(IVulnManager vulnManager, IProjectManager projectManager, ILogger<VulnController> logger, ITargetManager targetManager,
            IVulnCategoryManager vulnCategoryManager, IVulnNoteManager vulnNoteManager, IVulnAttachmentManager vulnAttachmentManager)
        {
            this.vulnManager = vulnManager;
            this.projectManager = projectManager;
            this.targetManager = targetManager;
            this.vulnCategoryManager = vulnCategoryManager;
            this.vulnAttachmentManager = vulnAttachmentManager;
            this.vulnNoteManager = vulnNoteManager;
            _logger = logger;
        }
        // GET: VulnController
        public ActionResult Index(int project)
        {
            try
            {
                VulnViewModel model = new VulnViewModel
                {
                    Project = projectManager.GetById(project),
                    Vulns = vulnManager.GetAll().Where(x => x.ProjectId == project)
                };
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }

        }

        // GET: VulnController/Details/5
        public ActionResult Details(int project, int id)
        {
            try
            {
                VulnDetailsViewModel model = new VulnDetailsViewModel
                {
                    Project = projectManager.GetById(project),
                    Vuln = vulnManager.GetById(id),
                    Notes = vulnNoteManager.GetAll().Where(x => x.VulnId == id),
                    Attachments = vulnAttachmentManager.GetAll().Where(x => x.VulnId == id)

                };
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error ocurred loading Vuln Workspace Details.Task: {0} Project: {1} User: {2}", id, project, User.FindFirstValue(ClaimTypes.Name));
                return View();
            }
        }

        // GET: VulnController/Create
        public ActionResult Create(int project)
        {
            try
            {

                var result = targetManager.GetAll().Where(x => x.ProjectId == project).Select(e => new VulnCreateViewModel
                {
                    TargetId = e.Id,
                    TargetName = e.Name,
                }).ToList();

                var targets = new List<SelectListItem>();

                foreach (var item in result)
                {
                    targets.Add(new SelectListItem { Text = item.TargetName, Value = item.TargetId.ToString() });
                }

                var result2 = vulnCategoryManager.GetAll().Select(e => new VulnCreateViewModel
                {
                    VulnCategoryId = e.Id,
                    VulnCategoryName = e.Name,
                }).ToList();

                var vulnCat = new List<SelectListItem>();

                foreach (var item in result2)
                {
                    vulnCat.Add(new SelectListItem { Text = item.VulnCategoryName, Value = item.VulnCategoryId.ToString() });
                }

                var model = new VulnCreateViewModel
                {
                    TargetList = targets,
                    VulnCatList = vulnCat
                };

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error ocurred loading Task Workspace create form.Project: {0} User: {1}", project, User.FindFirstValue(ClaimTypes.Name));
                return View();
            }
        }

        // POST: VulnController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int project, VulnCreateViewModel model)
        {
            try
            {
                Vuln vuln = new Vuln
                {
                    Name = model.Name,
                    ProjectId = project,
                    Template = model.Template,
                    cve = model.cve,
                    Description = model.Description,
                    VulnCategoryId = model.VulnCategoryId,
                    Risk = model.Risk,
                    Status = model.Status,
                    Impact = model.Impact,
                    TargetId = model.TargetId,
                    CVSS3 = model.CVSS3,
                    CVSSVector = model.CVSSVector,
                    ProofOfConcept = model.ProofOfConcept,
                    Remediation = model.Remediation,
                    RemediationComplexity = model.RemediationComplexity,
                    RemediationPriority = model.RemediationPriority,
                    CreatedDate = model.CreatedDate,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                vulnManager.Add(vuln);
                vulnManager.Context.SaveChanges();
                _logger.LogInformation("User: {0} Created a new Vuln on Project {1}", User.FindFirstValue(ClaimTypes.Name), project);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VulnController/Edit/5
        public ActionResult Edit(int project, int id)
        {
            try
            {
                var vulnResult = vulnManager.GetById(id);


                var result = targetManager.GetAll().Where(x => x.ProjectId == project).Select(e => new VulnCreateViewModel
                {
                    TargetId = e.Id,
                    TargetName = e.Name,
                }).ToList();

                var targets = new List<SelectListItem>();

                foreach (var item in result)
                {
                    targets.Add(new SelectListItem { Text = item.TargetName, Value = item.TargetId.ToString() });
                }

                var result2 = vulnCategoryManager.GetAll().Select(e => new VulnCreateViewModel
                {
                    VulnCategoryId = e.Id,
                    VulnCategoryName = e.Name,
                }).ToList();

                var vulnCat = new List<SelectListItem>();

                foreach (var item in result2)
                {
                    vulnCat.Add(new SelectListItem { Text = item.VulnCategoryName, Value = item.VulnCategoryId.ToString() });
                }


                VulnCreateViewModel model = new VulnCreateViewModel
                {
                    Name = vulnResult.Name,
                    Project = projectManager.GetById(project),
                    ProjectId = project,
                    Template = vulnResult.Template,
                    cve = vulnResult.cve,
                    Description = vulnResult.Description,
                    VulnCategoryId = vulnResult.VulnCategoryId,
                    Risk = vulnResult.Risk,
                    Status = vulnResult.Status,
                    Impact = vulnResult.Impact,
                    TargetId = vulnResult.TargetId,
                    CVSS3 = vulnResult.CVSS3,
                    CVSSVector = vulnResult.CVSSVector,
                    ProofOfConcept = vulnResult.ProofOfConcept,
                    Remediation = vulnResult.Remediation,
                    RemediationComplexity = vulnResult.RemediationComplexity,
                    RemediationPriority = vulnResult.RemediationPriority,
                    CreatedDate = vulnResult.CreatedDate,
                    UserId = vulnResult.UserId,
                    TargetList = targets,
                    VulnCatList = vulnCat
                };

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error ocurred loading Vuln Workspace edit PROJECT form.Project: {0} User: {1}", project, User.FindFirstValue(ClaimTypes.Name));
                return View();
            }
        }

        // POST: VulnController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int project, VulnCreateViewModel model, int id)
        {
            try
            {
                var result = vulnManager.GetById(id);
                result.Name = model.Name;
                result.ProjectId = project;
                result.Template = model.Template;
                result.cve = model.cve;
                result.Description = model.Description;
                result.VulnCategoryId = model.VulnCategoryId;
                result.Risk = model.Risk;
                result.Status = model.Status;
                result.Impact = model.Impact;
                result.TargetId = model.TargetId;
                result.CVSS3 = model.CVSS3;
                result.CVSSVector = model.CVSSVector;
                result.ProofOfConcept = model.ProofOfConcept;
                result.Remediation = model.Remediation;
                result.RemediationComplexity = model.RemediationComplexity;
                result.RemediationPriority = model.RemediationPriority;


                vulnManager.Context.SaveChanges();
                TempData["edited"] = "edited";
                _logger.LogInformation("User: {0} edited Vuln: {1} on Project {2}", User.FindFirstValue(ClaimTypes.Name), id, project);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error ocurred editing a Vuln Workspace on. Task: {0} Project: {1} User: {2}", id, project, User.FindFirstValue(ClaimTypes.Name));
                return View();
            }
        }

        // GET: VulnController/Delete/5
        public ActionResult Delete(int project, int id)
        {
            try
            {
                var result = vulnManager.GetById(id);
                return View(result);


            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error ocurred loading Vuln Workspace delete form. Project: {0} User: {1}", project, User.FindFirstValue(ClaimTypes.Name));
                return View("Index");
            }
        }

        // POST: VulnController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int project, int id, IFormCollection collection)
        {
            try
            {
                var result = vulnManager.GetById(id);
                if (result != null)
                {
                    vulnManager.Remove(result);
                    vulnManager.Context.SaveChanges();
                }

                TempData["deleted"] = "deleted";
                _logger.LogInformation("User: {0} deleted Vuln: {1} on Project {2}", User.FindFirstValue(ClaimTypes.Name), id, project);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error ocurred deleting a Vuln Workspace on. Task: {0} Project: {1} User: {2}", id, project, User.FindFirstValue(ClaimTypes.Name));
                return View();
            }
        }
    }
}
