using Microsoft.EntityFrameworkCore;
using NewEvent.Models;

namespace NewEvent.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Job> Jobs { get; set; }

    }
}
