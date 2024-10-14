// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using Microsoft.EntityFrameworkCore;
using ST10296167_PROG6212_POE.Models;

namespace ST10296167_PROG6212_POE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Claims> Claims { get; set; }

        public DbSet<Lecturers> Lecturers { get; set;}

        public DbSet<ProgrammeCoordinator> ProgrammeCoordinators { get; set; }

        public DbSet<AcademicManager> AcademicManagers { get; set;}

        public DbSet<Documents> Documents { get; set; }

    }
}
