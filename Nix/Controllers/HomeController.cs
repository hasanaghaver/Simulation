using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nix.Data;
using Nix.ViewModels;

namespace Nix.Controllers
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
                Tools = await _context.Tools.Where(t => !t.IsDeleted).ToListAsync()
            };


            return View(homeVM);
        }
    }
}
