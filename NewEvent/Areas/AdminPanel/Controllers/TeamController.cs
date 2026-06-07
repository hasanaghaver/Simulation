using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NewEvent.Areas.AdminPanel.ViewModels;
using NewEvent.Data;
using NewEvent.Models;
using NewEvent.Utilities;
using NewEvent.Utilities.Enums;
using System.Threading.Tasks;

namespace NewEvent.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class TeamController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetTeamVM> getTeamVMs = await context.Teams.Where(t => !t.IsDeleted)
                .Include(t => t.Job)
                .Select(t => new GetTeamVM
                {
                    Id = t.Id,
                    Image = t.Image,
                    Job = t.Job.Name,
                    Name = t.Name
                }).ToListAsync();

            return View(getTeamVMs);
        }

        public async Task<IActionResult> Create()
        {
            CreateTeamVM createTeamVM = new()
            {
                Jobs = await context.Jobs.Where(j => !j.IsDeleted).ToListAsync()
            };

            return View(createTeamVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamVM createTeamVM)
        {
            createTeamVM.Jobs = await context.Jobs.Where(j => !j.IsDeleted).ToListAsync();


            if (!ModelState.IsValid) return View(createTeamVM);

            if (!createTeamVM.Image.CheckType("image/"))
            {
                ModelState.AddModelError(nameof(createTeamVM.Image), "File type is incorrect");
                return View(createTeamVM);
            }
            if (!createTeamVM.Image.CheckSize(FileSize.Mb,2))
            {
                ModelState.AddModelError(nameof(createTeamVM.Image), "File must be less 2Mb");
                return View(createTeamVM);
            }

            Job? job = await context.Jobs.FirstOrDefaultAsync(j =>j.Id == createTeamVM.JobID);

            if (job is null)
            {
                ModelState.AddModelError(nameof(createTeamVM.JobID), "Please select job");
                return View(createTeamVM);
            }

            Team team = new()
            {
                Image = await createTeamVM.Image.CreateFile(env.WebRootPath, "images"),
                Name = createTeamVM.Name,
                Job = job
            };

            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Team? team = await context.Teams.Include(t=>t.Job).FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if (team is null) return NotFound();

            UpdateTeamVM updateTeamVM = new()
            {
                Image = team.Image,
                Name = team.Name,
                JobID = team.JobId,
                Jobs = await context.Jobs.Where(j => !j.IsDeleted).ToListAsync()
            };

            return View(updateTeamVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateTeamVM updateTeamVM)
        {
            updateTeamVM.Jobs = await context.Jobs.Where(j => !j.IsDeleted).ToListAsync();

            if (id is null || id < 1) return BadRequest();

            Team? team = await context.Teams.Include(t => t.Job).FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if (team is null) return NotFound();

            if (!ModelState.IsValid) return View(updateTeamVM);

            Job? job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == updateTeamVM.JobID);

            if (job is null)
            {
                ModelState.AddModelError(nameof(updateTeamVM.JobID), "Please select job");
                return View(updateTeamVM);
            }

            if (updateTeamVM.Photo is not null)
            {
                if (!updateTeamVM.Photo.CheckType("image/"))
                {
                    ModelState.AddModelError(nameof(updateTeamVM.Photo), "File type is incorrect");
                    return View(updateTeamVM);
                }
                if (!updateTeamVM.Photo.CheckSize(FileSize.Mb, 2))
                {
                    ModelState.AddModelError(nameof(updateTeamVM.Photo), "File must be less 2Mb");
                    return View(updateTeamVM);
                }

                team.Image.DeleteFile(env.WebRootPath, "images");

                team.Image = await updateTeamVM.Photo.CreateFile(env.WebRootPath, "images");
                
            }

            team.Name = updateTeamVM.Name;
            team.Job = job;

            context.Update(team);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async  Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Team? team = await context.Teams.Include(t => t.Job).FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if (team is null) return NotFound();

            context.Remove(team);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Team? team = await context.Teams.Include(t => t.Job).FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if (team is null) return NotFound();

            DetailTeamVm detailTeamVm = new()
            {
                Image = team.Image,
                Name = team.Name,
                Job = team.Job.Name
            };
            return View(detailTeamVm);
        }
    }
}
