using Microsoft.AspNetCore.Mvc;

namespace NewEvent.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
