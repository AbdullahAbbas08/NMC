using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moia.DAL.Migrations
{
    public partial class ChangeRoleNameIntoCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MainRoles",
                newName: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "MainRoles",
                newName: "Name");
        }
    }
}
