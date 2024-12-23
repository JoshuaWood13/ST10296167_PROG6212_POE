﻿// Name: Joshua Wood
// Student number: ST10296167
// Group: 2

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10296167_PROG6212_POE.Models;

namespace ST10296167_PROG6212_POE.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Claims> Claims { get; set; }

        public DbSet<Documents> Documents { get; set; }

    }
}
//--------------------------------------------------------X END OF FILE X-------------------------------------------------------------------//