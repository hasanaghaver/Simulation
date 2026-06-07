using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using NewEvent.Areas.AdminPanel.ViewModels;
using NewEvent.Data;
using NewEvent.Models;
using System.Threading.Tasks;

namespace NewEvent.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class JobController : Controller
    {
        private readonly AppDbContext context;

        public JobController(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetJobVM> getJobVMs = await context.Jobs.Where(j => !j.IsDeleted)
                .Select(j => new GetJobVM { Id = j.Id, Name = j.Name }).ToListAsync();

            return View(getJobVMs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobVM createJobVM)
        {
            if (!ModelState.IsValid) return View(createJobVM);

            Job job = new() { Name = createJobVM.Name };

            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Job? job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);

            if (job is null) return NotFound();

            UpdateJobVM updateJobVM = new()
            {
                Name = job.Name
            };

            return View(updateJobVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id , UpdateJobVM updateJobVM)
        {
            if (id is null || id < 1) return BadRequest();

            Job? job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);

            if (job is null) return NotFound();

            if (!ModelState.IsValid) return View();

            job.Name = updateJobVM.Name;

            context.Jobs.Update(job);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id is null || id < 1) return BadRequest();

            Job? job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);

            if (job is null) return NotFound();

            context.Jobs.Remove(job);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {

            if (id is null || id < 1) return BadRequest();

            Job? job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);

            if (job is null) return NotFound();

            DetailJobVM detailJobVM = new()
            {
                Name = job.Name
            };

            return View(detailJobVM);
        }
    }
}
