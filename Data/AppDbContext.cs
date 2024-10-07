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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Seeding data for Lecturer
        //    modelBuilder.Entity<Lecturers>().HasData(
        //        new Lecturers
        //        {
        //            LecturerID = 1,
        //            Password = "Lecturer123" 
        //        });

        //    // Seeding data for Programme Coordinator (PM)
        //    modelBuilder.Entity<ProgrammeCoordinator>().HasData(
        //        new ProgrammeCoordinator
        //        {
        //            PM_ID = 1,
        //            Password = "PM123"
        //        });

        //    // Seeding data for Academic Manager (AM)
        //    modelBuilder.Entity<AcademicManager>().HasData(
        //        new AcademicManager
        //        {
        //            AM_ID = 1,
        //            Password = "AM123"
        //        });
        //}
    }
}
