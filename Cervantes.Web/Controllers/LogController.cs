using Cervantes.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Cervantes.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LogController : Controller
    {
        private readonly ILogger<LogController> _logger = null;
        ILogManager logManager = null;

        public LogController(ILogger<LogController> logger, ILogManager logManager)
        {
            _logger = logger;
            this.logManager = logManager;
        }

        public ActionResult Index()
        {
            try
            {
                var model = logManager.GetAll();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred loading System Logs. by User: {0}", User.FindFirstValue(ClaimTypes.Name));
                return View();
            }


        }

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
