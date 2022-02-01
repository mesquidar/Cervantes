using Microsoft.AspNetCore.Mvc;

namespace Cervantes.Web.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Generate(int project)
        {
            return View();
        }
    }
}
