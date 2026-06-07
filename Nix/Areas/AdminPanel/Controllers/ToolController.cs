using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nix.Areas.AdminPanel.ViewModels;
using Nix.Data;
using Nix.Models;
using Nix.Utilities;
using Nix.Utilities.Enums;
using System.Threading.Tasks;

namespace Nix.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin,Moderator")]

    public class ToolController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ToolController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateToolVM createToolVM)
        {
            if (!ModelState.IsValid) return View(createToolVM);

            if (!createToolVM.Image.CheckType("image/"))
            {
                ModelState.AddModelError(nameof(createToolVM.Image), "File type is incorrect");
                return View(createToolVM);
            }
            if (!createToolVM.Image.CheckSize(FileSize.Mb,2))
            {
                ModelState.AddModelError(nameof(createToolVM.Image), "File size must be less 2Mb");
                return View(createToolVM);
            }

            Tools tool = new()
            {
                Image = await createToolVM.Image.CreateFile(_env.WebRootPath, "assets", "images"),
                Title = createToolVM.Title,
                SubTitle = createToolVM.SubTitle
            };

            await _context.Tools.AddAsync(tool);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Tools? tools = await _context.Tools.FirstOrDefaultAsync(t => t.Id == id || t.IsDeleted != true);

            if (tools is null) return NotFound();

            UpdateToolVM updateToolVM = new()
            {
                Image = tools.Image,
                Title = tools.Title,
                SubTitle = tools.SubTitle
            };

            return View(updateToolVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateToolVM updateToolVM)
        {
            if (id is null || id < 1) return BadRequest();

            Tools? tools = await _context.Tools.FirstOrDefaultAsync(t => t.Id == id || t.IsDeleted != true);

            if (tools is null) return NotFound();

            if (!ModelState.IsValid) return View(updateToolVM);

            if (updateToolVM.Photo is not null)
            {
                if (!updateToolVM.Photo.CheckType("image/"))
                {
                    ModelState.AddModelError(nameof(updateToolVM.Photo), "File type is incorrect");
                    return View(updateToolVM);
                }
                if (!updateToolVM.Photo.CheckSize(FileSize.Mb, 2))
                {
                    ModelState.AddModelError(nameof(updateToolVM.Photo), "File size must be less 2Mb");
                    return View(updateToolVM);
                }

                updateToolVM.Image.DeleteFile(_env.WebRootPath, "assets", "images");

                tools.Image = await updateToolVM.Photo.CreateFile(_env.WebRootPath, "assets", "images");

            }

            tools.Title = updateToolVM.Title;
            tools.SubTitle = updateToolVM.SubTitle;

            _context.Tools.Update(tools);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Tools? tools = await _context.Tools.FirstOrDefaultAsync(t => t.Id == id || t.IsDeleted != true);

            if (tools is null) return NotFound();

            tools.Image.DeleteFile(_env.WebRootPath, "assets", "images");

            _context.Remove(tools);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Tools? tools = await _context.Tools.FirstOrDefaultAsync(t => t.Id == id || t.IsDeleted != true);

            if (tools is null) return NotFound();

            DetailToolVM detailToolVM = new()
            {
                Image = tools.Image,
                Title = tools.Title,
                Subtitle = tools.SubTitle
                
            };

            return View(detailToolVM);

        }

    }
}
