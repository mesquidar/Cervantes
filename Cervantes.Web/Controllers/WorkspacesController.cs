using Cervantes.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Cervantes.Web.Controllers
{
    public class WorkspacesController : Controller
    {

        IProjectManager projectManager = null;
        IProjectUserManager projectUserManager = null;

        public WorkspacesController(IProjectManager projectManager, IProjectUserManager projectUserManager)
        {

            this.projectManager = projectManager;
            this.projectUserManager = projectUserManager;
        }
        // GET: WorkspaceController
        public IActionResult Index()
        {
            try
            {
                if (User.FindFirstValue(ClaimTypes.Role) == "Admin" | User.FindFirstValue(ClaimTypes.Role) == "SuperUser")
                {
                    var model = projectManager.GetAll();
                    return View(model);
                }
                else
                {
                    var projects = projectUserManager.GetAll().Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(y => y.ProjectId);
                    var model = projectManager.GetAll().Where(x => projects.Contains(x.Id));
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                return View();
            }
        }

    }
}
