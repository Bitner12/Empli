using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class AddContractorAndHourFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Hours",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ContractorId",
                table: "Hours",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contractors_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hours_ContractorId",
                table: "Hours",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_CompanyId",
                table: "Contractors",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hours_Contractors_ContractorId",
                table: "Hours",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hours_Contractors_ContractorId",
                table: "Hours");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropIndex(
                name: "IX_Hours_ContractorId",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "ContractorId",
                table: "Hours");
        }
    }
}
