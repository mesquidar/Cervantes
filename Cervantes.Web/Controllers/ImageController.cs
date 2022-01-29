using Microsoft.AspNetCore.Mvc;

namespace Cervantes.Web.Controllers
{
    public class ImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
