using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAssets.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeModelWalletAssetInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WaletAssets",
                table: "WaletAssets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WaletAssets");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "WaletAssets",
                newName: "Coin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaletAssets",
                table: "WaletAssets",
                columns: new[] { "Coin", "Wallet" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WaletAssets",
                table: "WaletAssets");

            migrationBuilder.RenameColumn(
                name: "Coin",
                table: "WaletAssets",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WaletAssets",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaletAssets",
                table: "WaletAssets",
                column: "Id");
        }
    }
}
