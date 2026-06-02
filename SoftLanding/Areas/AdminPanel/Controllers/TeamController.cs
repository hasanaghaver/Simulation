using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SoftLanding.Areas.AdminPanel.ViewModels;
using SoftLanding.Data;
using SoftLanding.Models;
using SoftLanding.Utilities;
using SoftLanding.Utilities.Enums;
using System.Threading.Tasks;

namespace SoftLanding.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin, Moderator")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetMember> getMember = await _context.TeamMembers
                .Where(tm => !tm.Isdeleted)
                .Select(tm=>new GetMember {
                    Id = tm.Id,
                    Image = tm.Image,
                    Name = tm.Name,
                    Job = tm.Job  
                })
                .ToListAsync();

            return View(getMember);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMember createMember)
        {
            if (!ModelState.IsValid) return View(createMember);

            if (!createMember.Image.CheckType("image/"))
            {
                ModelState.AddModelError(nameof(createMember.Image), "File type is incorrect!");
                return View(createMember);
            }
            if (!createMember.Image.CheckSize(FileSize.Mb,2))
            {
                ModelState.AddModelError(nameof(createMember.Image), "File size is incorrect!");
                return View(createMember);
            }

            TeamMembers teamMembers = new() 
            { 
                Image = await createMember.Image.CreateFile(_env.WebRootPath, "images"),
                Name = createMember.Name,
                Job = createMember.Job,
                Description = createMember.Description
            };

            await _context.TeamMembers.AddAsync(teamMembers);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            TeamMembers? teamMembers = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.Id == id);

            if (teamMembers is null) return NotFound();

            UpdateMember updateMember = new()
            {
                Image = teamMembers.Image,
                Name =teamMembers.Name,
                Job = teamMembers.Job,
                Description = teamMembers.Description
            };

            return View(updateMember);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateMember updateMember)
        {
            if (id is null || id < 1) return BadRequest();

            TeamMembers? teamMembers = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.Id == id);

            if (teamMembers is null) return NotFound();

            if (!ModelState.IsValid) return View(updateMember);

            if(updateMember.Photo is not null)
            {
                if (!updateMember.Photo.CheckType("image/"))
                {
                    ModelState.AddModelError(nameof(updateMember.Photo), "File type is incorrect!");
                    return View(updateMember);
                }
                if (!updateMember.Photo.CheckSize(FileSize.Mb, 2))
                {
                    ModelState.AddModelError(nameof(updateMember.Photo), "File size is incorrect!");
                    return View(updateMember);
                }

                updateMember.Image.DeleteFile(_env.WebRootPath, "images");

                teamMembers.Image = await updateMember.Photo.CreateFile(_env.WebRootPath, "images");


            }

            teamMembers.Name = updateMember.Name;
            teamMembers.Job = updateMember.Job;
            teamMembers.Description = updateMember.Description;

            _context.Update(teamMembers);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            TeamMembers? teamMembers = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.Id == id);

            if (teamMembers is null) return NotFound();

            teamMembers.Image.DeleteFile(_env.WebRootPath, "images");

            _context.Remove(teamMembers);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            TeamMembers? teamMembers = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.Id == id);

            if (teamMembers is null) return NotFound();

            DetailMember detailMember = new()
            {
                Name = teamMembers.Name,
                Image = teamMembers.Image,
                Job = teamMembers.Job,
                Description = teamMembers.Description
            };

            return View(detailMember);
        }
    }
}
