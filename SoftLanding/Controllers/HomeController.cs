using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftLanding.Data;
using SoftLanding.ViewModel;

namespace SoftLanding.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new()
            {
                TeamMembers = await _context.TeamMembers.Where(tm => !tm.Isdeleted).ToListAsync()
            };

            return View(homeVM);
        }
    }
}
