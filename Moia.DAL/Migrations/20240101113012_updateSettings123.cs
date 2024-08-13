using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moia.DAL.Migrations
{
    public partial class updateSettings123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MainUserRole_BranshId",
                table: "MainUserRole");

            migrationBuilder.AddColumn<int>(
                name: "UserRoleID",
                table: "MinistryBranshs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MinistryBranshs_UserRoleID",
                table: "MinistryBranshs",
                column: "UserRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_MainUserRole_BranshId",
                table: "MainUserRole",
                column: "BranshId");

            migrationBuilder.AddForeignKey(
                name: "FK_MinistryBranshs_MainUserRole_UserRoleID",
                table: "MinistryBranshs",
                column: "UserRoleID",
                principalTable: "MainUserRole",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinistryBranshs_MainUserRole_UserRoleID",
                table: "MinistryBranshs");

            migrationBuilder.DropIndex(
                name: "IX_MinistryBranshs_UserRoleID",
                table: "MinistryBranshs");

            migrationBuilder.DropIndex(
                name: "IX_MainUserRole_BranshId",
                table: "MainUserRole");

            migrationBuilder.DropColumn(
                name: "UserRoleID",
                table: "MinistryBranshs");

            migrationBuilder.CreateIndex(
                name: "IX_MainUserRole_BranshId",
                table: "MainUserRole",
                column: "BranshId",
                unique: true,
                filter: "[BranshId] IS NOT NULL");
        }
    }
}
