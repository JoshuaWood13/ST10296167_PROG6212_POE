﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ST10296167_PROG6212_POE.Data;

#nullable disable

namespace ST10296167_PROG6212_POE.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241007132340_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ST10296167_PROG6212_POE.Models.AcademicManager", b =>
                {
                    b.Property<int>("LecturerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LecturerID"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("LecturerID");

                    b.ToTable("AcademicManager");
                });

            modelBuilder.Entity("ST10296167_PROG6212_POE.Models.Claims", b =>
                {
                    b.Property<int>("ClaimID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClaimID"));

                    b.Property<int>("ApprovalAM")
                        .HasColumnType("int");

                    b.Property<int>("ApprovalPC")
                        .HasColumnType("int");

                    b.Property<double>("ClaimAmount")
                        .HasColumnType("float");

                    b.Property<DateTime>("ClaimMonth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("HourlyRate")
                        .HasColumnType("float");

                    b.Property<double>("HoursWorked")
                        .HasColumnType("float");

                    b.Property<int>("LecturerID")
                        .HasColumnType("int");

                    b.HasKey("ClaimID");

                    b.HasIndex("LecturerID");

                    b.ToTable("Claims");
                });

            modelBuilder.Entity("ST10296167_PROG6212_POE.Models.Documents", b =>
                {
                    b.Property<int>("DocumentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentID"));

                    b.Property<int>("ClaimID")
                        .HasColumnType("int");

                    b.Property<byte[]>("FileData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DocumentID");

                    b.HasIndex("ClaimID");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("ST10296167_PROG6212_POE.Models.Lecturers", b =>
                {
                    b.Property<int>("LecturerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LecturerID"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("LecturerID");

                    b.ToTable("Lecturers");
                });

            modelBuilder.Entity("ST10296167_PROG6212_POE.Models.ProgrammeCoordinator", b =>
                {
                    b.Property<int>("LecturerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LecturerID"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("LecturerID");

                    b.ToTable("ProgrammeCoordinators");
                });

            modelBuilder.Entity("ST10296167_PROG6212_POE.Models.Claims", b =>
                {
                    b.HasOne("ST10296167_PROG6212_POE.Models.Lecturers", "Lecturers")
                        .WithMany()
                        .HasForeignKey("LecturerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lecturers");
                });

            modelBuilder.Entity("ST10296167_PROG6212_POE.Models.Documents", b =>
                {
                    b.HasOne("ST10296167_PROG6212_POE.Models.Claims", "Claims")
                        .WithMany()
                        .HasForeignKey("ClaimID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Claims");
                });
#pragma warning restore 612, 618
        }
    }
}
