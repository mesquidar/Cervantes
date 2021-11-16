using Microsoft.EntityFrameworkCore.Migrations;

namespace Cervantes.DAL.Migrations
{
    public partial class AddedOrganizationImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Organization",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Organization");
        }
    }
}
