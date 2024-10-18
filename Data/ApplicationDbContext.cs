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

        // The DbSet Property for the Claims Model
        public DbSet<Claims> Claims { get; set; }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//