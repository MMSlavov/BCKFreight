using Microsoft.EntityFrameworkCore.Migrations;

namespace BCKFreightTMS.Data.Migrations
{
    public partial class OrderDocumentationsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecievedDocumentationId",
                table: "Documentations",
                type: "int",
                nullable: true);

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
                name: "IX_Documentations_RecievedDocumentationId",
                table: "Documentations");

            migrationBuilder.DropColumn(
                name: "RecievedDocumentationId",
                table: "Documentations");
        }
    }
}
