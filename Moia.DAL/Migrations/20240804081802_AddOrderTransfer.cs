using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moia.DAL.Migrations
{
    public partial class AddOrderTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderTransfereID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderTransfere",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUserId = table.Column<int>(type: "int", nullable: false),
                    ToUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTransfere", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderTransfere_MainUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "MainUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTransfere_MainUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "MainUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTransfereID",
                table: "Orders",
                column: "OrderTransfereID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTransfere_FromUserId",
                table: "OrderTransfere",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTransfere_ToUserId",
                table: "OrderTransfere",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTransfere_OrderTransfereID",
                table: "Orders",
                column: "OrderTransfereID",
                principalTable: "OrderTransfere",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTransfere_OrderTransfereID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderTransfere");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderTransfereID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderTransfereID",
                table: "Orders");
        }
    }
}
