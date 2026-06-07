using Microsoft.EntityFrameworkCore;
using Nix.Models;

namespace Nix.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Tools> Tools { get; set; }
    }
}
