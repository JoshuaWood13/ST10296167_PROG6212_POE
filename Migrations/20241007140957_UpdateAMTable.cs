using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10296167_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAMTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AcademicManager",
                table: "AcademicManager");

            migrationBuilder.RenameTable(
                name: "AcademicManager",
                newName: "AcademicManagers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcademicManagers",
                table: "AcademicManagers",
                column: "AM_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AcademicManagers",
                table: "AcademicManagers");

            migrationBuilder.RenameTable(
                name: "AcademicManagers",
                newName: "AcademicManager");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcademicManager",
                table: "AcademicManager",
                column: "AM_ID");
        }
    }
}
