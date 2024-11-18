// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;
using ST10296167_PROG6212_POE.Services;

namespace ST10296167_PROG6212_POE
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
             {
                    // Custom control, so no need for default redirect paths
             });

            builder.Services.AddRazorPages();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            // Seeding roles and users
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Add roles if they don't exist
                var roleNames = new[] { "Lecturer", "Programme Coordinator", "Academic Manager", "Human Resources" };
                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Seed default users if they don't exist
                if (!dbContext.Users.Any())
                {
                    // Create default users
                    var lecturer = new User
                    {
                        Id = "L001",
                        UserName = "lecturer@example.com",
                        Email = "lecturer@example.com",
                        FirstName = "John",
                        LastName = "Doe",
                        PhoneNumber = "0713456732",
                        Address = "22 Washington St",
                    };

                    var pc = new User
                    {
                        Id = "PC001",
                        UserName = "pc@example.com",
                        Email = "pc@example.com",
                        FirstName = "Jane",
                        LastName = "Doe",
                        PhoneNumber = "0794732159",
                        Address = "10 Sidewalk Av",
                    };

                    var am = new User
                    {
                        Id = "AM001",
                        UserName = "am@example.com",
                        Email = "am@example.com",
                        FirstName = "Bob",
                        LastName = "Snarby",
                        PhoneNumber = "0798563241",
                        Address = "5 Main St",
                    };

                    var hr = new User
                    {
                        Id = "HR001",
                        UserName = "hr@example.com",
                        Email = "hr@example.com",
                        FirstName = "Mary",
                        LastName = "Jane",
                        PhoneNumber = "0786452398",
                        Address = "12 Gotham Ave",
                    };

                    // Create users with default passwords
                    var lecturerResult = await userManager.CreateAsync(lecturer, "Lecturer123!");
                    if (lecturerResult.Succeeded)
                    {
                        // Directly add users to roles if user creation succeeded
                        await userManager.AddToRoleAsync(lecturer, "Lecturer");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create lecturer: " + string.Join(", ", lecturerResult.Errors.Select(e => e.Description)));
                    }

                    var pcResult = await userManager.CreateAsync(pc, "Pc123!");
                    if (pcResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(pc, "Programme Coordinator");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create PC: " + string.Join(", ", pcResult.Errors.Select(e => e.Description)));
                    }

                    var amResult = await userManager.CreateAsync(am, "Am123!");
                    if (amResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(am, "Academic Manager");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create AM: " + string.Join(", ", amResult.Errors.Select(e => e.Description)));
                    }

                    var hrResult = await userManager.CreateAsync(hr, "Hr123!");
                    if (hrResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(hr, "Human Resources");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create hr: " + string.Join(", ", hrResult.Errors.Select(e => e.Description)));
                    }

                }
            }





            //Old seeding
            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //    // Seed initial data
            //    if (!dbContext.Lecturers.Any())
            //    {
            //        dbContext.Lecturers.Add(new Lecturers
            //        {
            //            Password = "Lecturer123"
            //        });
            //    }

            //    if (!dbContext.ProgrammeCoordinators.Any())
            //    {
            //        dbContext.ProgrammeCoordinators.Add(new ProgrammeCoordinator
            //        {
            //            Password = "PM123"
            //        });
            //    }

            //    if (!dbContext.AcademicManagers.Any())
            //    {
            //        dbContext.AcademicManagers.Add(new AcademicManager
            //        {
            //            Password = "AM123"
            //        });
            //    }

            //    dbContext.SaveChanges(); 
            //}

            app.Run();
        }
    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//