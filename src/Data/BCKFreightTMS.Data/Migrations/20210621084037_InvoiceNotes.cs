namespace BCKFreightTMS.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InvoiceNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "InvoiceOut",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceNoteId",
                table: "InvoiceOut",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceNoteType",
                table: "InvoiceOut",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceOutId",
                table: "InvoiceOut",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceInId",
                table: "InvoiceIn",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceNoteId",
                table: "InvoiceIn",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NoteInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNoteType = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceOut_InvoiceNoteId",
                table: "InvoiceOut",
                column: "InvoiceNoteId",
                unique: true,
                filter: "[InvoiceNoteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceOut_InvoiceOutId",
                table: "InvoiceOut",
                column: "InvoiceOutId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceIn_InvoiceInId",
                table: "InvoiceIn",
                column: "InvoiceInId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceIn_InvoiceNoteId",
                table: "InvoiceIn",
                column: "InvoiceNoteId",
                unique: true,
                filter: "[InvoiceNoteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NoteInfo_IsDeleted",
                table: "NoteInfo",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_InvoiceIn_InvoiceInId",
                table: "InvoiceIn",
                column: "InvoiceInId",
                principalTable: "InvoiceIn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_NoteInfo_InvoiceNoteId",
                table: "InvoiceIn",
                column: "InvoiceNoteId",
                principalTable: "NoteInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_InvoiceOut_InvoiceOutId",
                table: "InvoiceOut",
                column: "InvoiceOutId",
                principalTable: "InvoiceOut",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_NoteInfo_InvoiceNoteId",
                table: "InvoiceOut",
                column: "InvoiceNoteId",
                principalTable: "NoteInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_InvoiceIn_InvoiceInId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_NoteInfo_InvoiceNoteId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_InvoiceOut_InvoiceOutId",
                table: "InvoiceOut");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_NoteInfo_InvoiceNoteId",
                table: "InvoiceOut");

            migrationBuilder.DropTable(
                name: "NoteInfo");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceOut_InvoiceNoteId",
                table: "InvoiceOut");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceOut_InvoiceOutId",
                table: "InvoiceOut");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceIn_InvoiceInId",
                table: "InvoiceIn");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceIn_InvoiceNoteId",
                table: "InvoiceIn");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "InvoiceOut");

            migrationBuilder.DropColumn(
                name: "InvoiceNoteId",
                table: "InvoiceOut");

            migrationBuilder.DropColumn(
                name: "InvoiceNoteType",
                table: "InvoiceOut");

            migrationBuilder.DropColumn(
                name: "InvoiceOutId",
                table: "InvoiceOut");

            migrationBuilder.DropColumn(
                name: "InvoiceInId",
                table: "InvoiceIn");

            migrationBuilder.DropColumn(
                name: "InvoiceNoteId",
                table: "InvoiceIn");
        }
    }
}
