using Microsoft.EntityFrameworkCore.Migrations;

namespace Cervantes.DAL.Migrations
{
    public partial class AddedVulnSatatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Vulns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Vulns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Vulns");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Vulns");
        }
    }
}
