using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10296167_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class ProperCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LecturerID",
                table: "ProgrammeCoordinators",
                newName: "PM_ID");

            migrationBuilder.RenameColumn(
                name: "LecturerID",
                table: "AcademicManager",
                newName: "AM_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PM_ID",
                table: "ProgrammeCoordinators",
                newName: "LecturerID");

            migrationBuilder.RenameColumn(
                name: "AM_ID",
                table: "AcademicManager",
                newName: "LecturerID");
        }
    }
}
