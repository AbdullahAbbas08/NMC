using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moia.DAL.Migrations
{
    public partial class AddActiveIntoUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "MainUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "MainUsers");
        }
    }
}
