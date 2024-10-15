using Microsoft.EntityFrameworkCore;
using ST10384311PROG6212POE.Models;

namespace ST10384311PROG6212POE.Data
{
    public class ApplicationDbContext : DbContext
    {
        // The Constructor Method for the ApplicationDbContext Class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // The DbSet Properties for the ApplicationDbContext Class

        // The DbSet Property for the Claims Model
        public DbSet<Claims> Claims { get; set; }

        /*// The DbSet Property for the Employee Model
        public DbSet<Employee> Employee{ get; set; }

        // The DbSet Property for the Lecturer Model
        public DbSet<Lecturer> Lecturer{ get; set; }

        // The DbSet Property for the AcademicManager Model
        public DbSet<AcademicManager> AcademicManager { get; set; }

        // The DbSet Property for the Programme Coordinator Model
        public DbSet<ProgrammeCoordinator> ProgrammeCoordinator { get; set; }*/
    }
}
