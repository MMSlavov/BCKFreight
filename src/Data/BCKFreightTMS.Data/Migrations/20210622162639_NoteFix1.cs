namespace BCKFreightTMS.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class NoteFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_InvoiceIn_InvoiceInId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_InvoiceOut_InvoiceOutId",
                table: "InvoiceOut");

            migrationBuilder.RenameColumn(
                name: "InvoiceOutId",
                table: "InvoiceOut",
                newName: "InvoiceNoteOutId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOut_InvoiceOutId",
                table: "InvoiceOut",
                newName: "IX_InvoiceOut_InvoiceNoteOutId");

            migrationBuilder.RenameColumn(
                name: "InvoiceInId",
                table: "InvoiceIn",
                newName: "InvoiceNoteInId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_InvoiceInId",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_InvoiceNoteInId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_InvoiceIn_InvoiceNoteInId",
                table: "InvoiceIn",
                column: "InvoiceNoteInId",
                principalTable: "InvoiceIn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_InvoiceOut_InvoiceNoteOutId",
                table: "InvoiceOut",
                column: "InvoiceNoteOutId",
                principalTable: "InvoiceOut",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_InvoiceIn_InvoiceNoteInId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_InvoiceOut_InvoiceNoteOutId",
                table: "InvoiceOut");

            migrationBuilder.RenameColumn(
                name: "InvoiceNoteOutId",
                table: "InvoiceOut",
                newName: "InvoiceOutId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOut_InvoiceNoteOutId",
                table: "InvoiceOut",
                newName: "IX_InvoiceOut_InvoiceOutId");

            migrationBuilder.RenameColumn(
                name: "InvoiceNoteInId",
                table: "InvoiceIn",
                newName: "InvoiceInId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_InvoiceNoteInId",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_InvoiceInId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_InvoiceIn_InvoiceInId",
                table: "InvoiceIn",
                column: "InvoiceInId",
                principalTable: "InvoiceIn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_InvoiceOut_InvoiceOutId",
                table: "InvoiceOut",
                column: "InvoiceOutId",
                principalTable: "InvoiceOut",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
