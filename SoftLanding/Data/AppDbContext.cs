using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftLanding.Models;

namespace SoftLanding.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<TeamMembers> TeamMembers { get; set; }
    }
}
