using Cervantes.Contracts;
using Cervantes.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace Cervantes.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;
        IProjectManager projectManager = null;
        IClientManager clientManager = null;
        IVulnManager vulnManager = null;
        ITaskManager taskManager = null;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer, IProjectManager projectManager, IClientManager clientManager, IVulnManager vulnManager, ITaskManager taskManager)
        {
            _logger = logger;
            _localizer = localizer;
            this.projectManager = projectManager;
            this.clientManager = clientManager;
            this.vulnManager = vulnManager;
            this.taskManager = taskManager;
        }

        public IActionResult Index()
        {
            try
            {
                DashboardViewModel model = new DashboardViewModel
                {
                    ProjectNumber = projectManager.GetAll().Count(),
                    VulnNumber = vulnManager.GetAll().Count(),
                    ClientNumber = clientManager.GetAll().Count(),
                    TasksNumber = taskManager.GetAll().Count(),
                    ActiveProjects = projectManager.GetAll().Where(x => x.Status == CORE.ProjectStatus.Active),
                    RecentClients = clientManager.GetAll().OrderByDescending(x => x.Id).Take(5),
                };

                return View(model);

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult OnGetSetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
