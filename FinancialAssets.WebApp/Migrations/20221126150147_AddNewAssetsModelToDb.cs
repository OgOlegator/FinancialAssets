using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAssets.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNewAssetsModelToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,12)", nullable: false),
                    Count = table.Column<decimal>(type: "decimal(18,12)", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Marketplace = table.Column<string>(type: "nvarchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");
        }
    }
}
