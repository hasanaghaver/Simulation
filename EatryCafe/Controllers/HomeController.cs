using System.Diagnostics;
using System.Threading.Tasks;
using EatryCafe.Data;
using EatryCafe.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EatryCafe.Controllers
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
                Chefs = await _context.Chefs.Where(c => !c.IsDeleted).ToListAsync()
            };

            return View(homeVM);
        }

    }
}
