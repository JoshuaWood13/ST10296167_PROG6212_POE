using Microsoft.EntityFrameworkCore;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;

namespace ST10296167_PROG6212_POE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Seed initial data
                if (!dbContext.Lecturers.Any())
                {
                    dbContext.Lecturers.Add(new Lecturers
                    {
                        //LecturerID = 1,
                        Password = "Lecturer123"
                    });
                }

                if (!dbContext.ProgrammeCoordinators.Any())
                {
                    dbContext.ProgrammeCoordinators.Add(new ProgrammeCoordinator
                    {
                        //PM_ID = 1,
                        Password = "PM123"
                    });
                }

                if (!dbContext.AcademicManagers.Any())
                {
                    dbContext.AcademicManagers.Add(new AcademicManager
                    {
                        //AM_ID = 1,
                        Password = "AM123"
                    });
                }

                dbContext.SaveChanges(); // Save all changes to the database.
            }

            app.Run();
        }
    }
}
