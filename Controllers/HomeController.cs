using Microsoft.AspNetCore.Mvc;

namespace TravelWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
