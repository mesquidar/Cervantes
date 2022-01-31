using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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
                var logs = logManager.GetAll().Select(e => new Log
                {
                    Id = e.Id,
                    Level = e.Level,
                    StackTrace = e.StackTrace,
                    Message = e.Message,
                    CreatedOn = e.CreatedOn,
                    Exception = e.Exception,
                    Url = e.Url,
                    Logger = e.Logger
                });

                LogViewModel model = new LogViewModel
                {
                    Logs = logs,
                };
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

        public ActionResult Export()
        {
            var logs = logManager.GetAll().Select(e => new Log
            {
                Id = e.Id,
                Level = e.Level,
                StackTrace = e.StackTrace,
                Message = e.Message,
                CreatedOn = e.CreatedOn,
                Exception = e.Exception,
                Url = e.Url,
                Logger = e.Logger
            });

            string jsonString = JsonSerializer.Serialize(logs);
            var fileName = "logs-export-" + DateTime.Now.ToString() + ".json";
            var mimeType = "text/plain";
            var fileBytes = Encoding.ASCII.GetBytes(jsonString);
            return new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = fileName
            };
        }
    }
}
