// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using Microsoft.EntityFrameworkCore;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;
using ST10296167_PROG6212_POE.Services;

namespace ST10296167_PROG6212_POE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registering HttpClient without a base address
            builder.Services.AddHttpClient<ClaimApiService>(client =>
            {
                // No need to set a base address, we're using relative URIs in the service
            });

           // builder.Services.AddHttpClient();

            builder.Services.AddScoped<ClaimApiService>();

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
                        Password = "Lecturer123"
                    });
                }

                if (!dbContext.ProgrammeCoordinators.Any())
                {
                    dbContext.ProgrammeCoordinators.Add(new ProgrammeCoordinator
                    {
                        Password = "PM123"
                    });
                }

                if (!dbContext.AcademicManagers.Any())
                {
                    dbContext.AcademicManagers.Add(new AcademicManager
                    {
                        Password = "AM123"
                    });
                }

                dbContext.SaveChanges(); 
            }

            app.Run();
        }
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//