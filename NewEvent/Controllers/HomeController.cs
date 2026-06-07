using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewEvent.Data;
using NewEvent.ViewModels;

namespace NewEvent.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

        public HomeController(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new()
            {
                Teams = await context.Teams.Include(t => t.Job).Where(t => !t.IsDeleted).ToListAsync()
            };

            return View(homeVM);
        }

    }
}
