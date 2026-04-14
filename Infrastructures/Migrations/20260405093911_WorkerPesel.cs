using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class WorkerPesel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pesel",
                table: "Workers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pesel",
                table: "Employees",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_Pesel",
                table: "Workers",
                column: "Pesel");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Pesel",
                table: "Employees",
                column: "Pesel",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workers_Pesel",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Pesel",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Pesel",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Pesel",
                table: "Employees");
        }
    }
}
