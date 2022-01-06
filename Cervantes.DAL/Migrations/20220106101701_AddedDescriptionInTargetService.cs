using Microsoft.EntityFrameworkCore.Migrations;

namespace Cervantes.DAL.Migrations
{
    public partial class AddedDescriptionInTargetService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TargetServices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TargetServices");
        }
    }
}
