using Microsoft.EntityFrameworkCore.Migrations;

namespace BCKFreightTMS.Data.Migrations
{
    public partial class OrdersFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderFromId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderToId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "OrderToId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OrderFromId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RecievedDocumentationId",
                table: "Documentations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderFromId",
                table: "Orders",
                column: "OrderFromId",
                unique: true,
                filter: "[OrderFromId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderToId",
                table: "Orders",
                column: "OrderToId",
                unique: true,
                filter: "[OrderToId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documentations_RecievedDocumentationId",
                table: "Documentations",
                column: "RecievedDocumentationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documentations_Documentations_RecievedDocumentationId",
                table: "Documentations",
                column: "RecievedDocumentationId",
                principalTable: "Documentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentations_Documentations_RecievedDocumentationId",
                table: "Documentations");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderFromId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderToId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Documentations_RecievedDocumentationId",
                table: "Documentations");

            migrationBuilder.DropColumn(
                name: "RecievedDocumentationId",
                table: "Documentations");

            migrationBuilder.AlterColumn<int>(
                name: "OrderToId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderFromId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderFromId",
                table: "Orders",
                column: "OrderFromId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderToId",
                table: "Orders",
                column: "OrderToId",
                unique: true);
        }
    }
}
