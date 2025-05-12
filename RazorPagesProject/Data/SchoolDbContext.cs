using Microsoft.EntityFrameworkCore;
using RazorPagesProject.Models;

namespace RazorPagesProject.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<Class> Classes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global query filter: IsActive = true olanları getir
            modelBuilder.Entity<Class>().HasQueryFilter(c => c.IsActive);
        }
    }
}
