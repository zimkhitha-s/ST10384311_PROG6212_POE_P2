using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10384311PROG6212POE.Models.Entities;

namespace ST10384311PROG6212POE.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string EmployeeRole { get; set; } // Role can be Lecturer, Admin, etc.
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Claims> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lecturer>()
                .HasMany(l => l.Claims)
                .WithOne(c => c.Lecturer)
                .HasForeignKey(c => c.LecturerId);
        }
    }

}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//