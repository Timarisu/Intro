using Intro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Intro.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
