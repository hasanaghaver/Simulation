using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nix.Areas.AdminPanel.ViewModels;
using Nix.Data;
using System.Threading.Tasks;

namespace Nix.Areas.AdminPanel.Controllers
{
    public class ToolController : Controller
    {
        private readonly AppDbContext _context;

        public ToolController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetToolVM> getToolVMs = await _context.Tools
                .Where(t=> !t.IsDeleted)
                .Select(t=> new GetToolVM()
                {
                    Id= t.Id,
                    Image = t.Image,
                    Title = t.Title,
                    SubTitle = t.SubTitle
                })
                .ToListAsync();
            return View(getToolVMs);
        }
    }
}
