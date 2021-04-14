namespace BCKFreightTMS.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class VATReasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VATReasonId",
                table: "InvoiceOut",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VATReasonId",
                table: "InvoiceIn",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VATReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VATReasons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceOut_VATReasonId",
                table: "InvoiceOut",
                column: "VATReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceIn_VATReasonId",
                table: "InvoiceIn",
                column: "VATReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_VATReasons_IsDeleted",
                table: "VATReasons",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_VATReasons_VATReasonId",
                table: "InvoiceIn",
                column: "VATReasonId",
                principalTable: "VATReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_VATReasons_VATReasonId",
                table: "InvoiceOut",
                column: "VATReasonId",
                principalTable: "VATReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_VATReasons_VATReasonId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_VATReasons_VATReasonId",
                table: "InvoiceOut");

            migrationBuilder.DropTable(
                name: "VATReasons");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceOut_VATReasonId",
                table: "InvoiceOut");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceIn_VATReasonId",
                table: "InvoiceIn");

            migrationBuilder.DropColumn(
                name: "VATReasonId",
                table: "InvoiceOut");

            migrationBuilder.DropColumn(
                name: "VATReasonId",
                table: "InvoiceIn");
        }
    }
}
