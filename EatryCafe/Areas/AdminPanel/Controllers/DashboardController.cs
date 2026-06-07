using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatryCafe.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles ="Admin,Moderator")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
