using Microsoft.EntityFrameworkCore;
using ST10384311PROG6212POE.Models.Entities;

namespace ST10384311PROG6212POE.Data
{
    public class ApplicationDbContext : DbContext
    {
        // The Constructor Method for the ApplicationDbContext Class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // The DbSet Properties for the ApplicationDbContext Class

        // The DbSet Property for the Employee Model
        public DbSet<Employee> Employees { get; set; }

        // The DbSet Property for the Lecturer Model
        public DbSet<Lecturer> Lecturers { get; set; }

        // The DbSet Property for the Claims Model
        public DbSet<Claims> Claims { get; set; }

        // The OnModelCreating Method for the ApplicationDbContext Class
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Lecturer-Claims Relationship
            modelBuilder.Entity<Lecturer>()
                .HasMany(l => l.Claims)
                .WithOne(c => c.Lecturer)
                .HasForeignKey(c => c.LecturerId);
        }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//