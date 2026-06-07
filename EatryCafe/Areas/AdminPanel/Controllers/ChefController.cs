using EatryCafe.Areas.AdminPanel.ViewModels;
using EatryCafe.Data;
using EatryCafe.Models;
using EatryCafe.Utilities;
using EatryCafe.Utilities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EatryCafe.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ChefController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ChefController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<IActionResult> Index()
        {
            List<GetChefVM> getChefVMs = await _context.Chefs
                .Where(c => !c.IsDeleted)
                .Select(c => new GetChefVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    Image = c.Image,
                    Job = c.Job
                }).ToListAsync();

            return View(getChefVMs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateChefVM createChefVM)
        {
            if (!ModelState.IsValid) return View(createChefVM);

            if (!createChefVM.Image.CheckType("image/"))
            {
                ModelState.AddModelError(nameof(createChefVM.Image), "File type must be image");
                return View(createChefVM);
            }
            if (!createChefVM.Image.CheckSize(FileSize.Mb, 2))
            {
                ModelState.AddModelError(nameof(createChefVM.Image), "File size must be max 200KB");
                return View(createChefVM);
            }

            Chefs chefs = new Chefs
            {
                Name = createChefVM.Name,
                Job = createChefVM.Job,
                Description = createChefVM.Description,
                Image = await createChefVM.Image.CreateFile(_env.WebRootPath, "images")
            };

            await _context.Chefs.AddAsync(chefs);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            Chefs? chefs = await _context.Chefs.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (chefs == null) return NotFound();

            UpdateChefVm updateChefVm = new()
            {
                Image = chefs.Image,
                Name = chefs.Name,
                Job = chefs.Job,
                Description = chefs.Description
            };

            return View(updateChefVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateChefVm updateChefVm)
        {
            if (id == null || id < 1) return BadRequest();

            Chefs? chefs = _context.Chefs.FirstOrDefault(c => c.Id == id && !c.IsDeleted);

            if (chefs == null) return NotFound();

            if (!ModelState.IsValid) return View(updateChefVm);


            if (updateChefVm.Photo != null)
            {
                if (!updateChefVm.Photo.CheckType("image/"))
                {
                    ModelState.AddModelError(nameof(updateChefVm.Photo), "File type must be image");
                    return View(updateChefVm);
                }
                if (!updateChefVm.Photo.CheckSize(FileSize.Mb, 2))
                {
                    ModelState.AddModelError(nameof(updateChefVm.Photo), "File size must be max 200KB");
                    return View(updateChefVm);
                }
                updateChefVm.Image.DeleteFile(_env.WebRootPath, "images");

                chefs.Image = await updateChefVm.Photo.CreateFile(_env.WebRootPath, "images");
            }


            chefs.Name = updateChefVm.Name;
            chefs.Job = updateChefVm.Job;
            chefs.Description = updateChefVm.Description;

            _context.Chefs.Update(chefs);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            Chefs? chefs = await _context.Chefs.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (chefs == null) return NotFound();

            chefs.Image.DeleteFile(_env.WebRootPath, "images");

            _context.Chefs.Remove(chefs);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            Chefs? chefs = await _context.Chefs.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (chefs == null) return NotFound();

            DetailChefVm detailChefVm = new()
            {
                Image = chefs.Image,
                Name = chefs.Name,
                Job = chefs.Job,
                Description = chefs.Description
            };

            return View(detailChefVm);
        }
    }
}
