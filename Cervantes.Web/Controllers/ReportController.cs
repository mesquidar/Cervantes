using Microsoft.AspNetCore.Mvc;

namespace Cervantes.Web.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
