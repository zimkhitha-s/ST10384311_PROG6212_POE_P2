using Microsoft.EntityFrameworkCore;
using ST10384311PROG6212POE.Models;

namespace ST10384311PROG6212POE.Database
{
    public class CMCSApplicationDbContext : DbContext
    {
        // The Constructor Method for the CMCSApplicationDbContext Class
        public CMCSApplicationDbContext(DbContextOptions<CMCSApplicationDbContext> options) : base(options)
        {
        }

        // The DbSet Properties for the CMCSApplicationDbContext Class

        // The DbSet Property for the EmployeeModel Class
        public DbSet<EmployeeModel> EmployeeModel { get; set; }

        // The DbSet Property for the AcademicManagerModel Class
        public DbSet<LecturerModel> LecturerModel { get; set; }

        // The DbSet Property for the PartTimeLectureModel Class
        public DbSet<PartTimeLectureModel> PartTimeLectureModel { get; set; }

        // The DbSet Property for the FullTimeLectureModel Class
        public DbSet<FullTimeLectureModel> FullTimeLectureModel { get; set; }
    }
}
