using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BCKFreightTMS.Data.Migrations
{
    public partial class BankMovements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankMovementId",
                table: "InvoiceOut",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankMovementId",
                table: "InvoiceIn",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovementType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankMovements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OppositeSideName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OppositeSideAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountingTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankMovements_AccountingTypes_AccountingTypeId",
                        column: x => x.AccountingTypeId,
                        principalTable: "AccountingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceOut_BankMovementId",
                table: "InvoiceOut",
                column: "BankMovementId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceIn_BankMovementId",
                table: "InvoiceIn",
                column: "BankMovementId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingTypes_IsDeleted",
                table: "AccountingTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_BankMovements_AccountingTypeId",
                table: "BankMovements",
                column: "AccountingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BankMovements_IsDeleted",
                table: "BankMovements",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_BankMovements_BankMovementId",
                table: "InvoiceIn",
                column: "BankMovementId",
                principalTable: "BankMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_BankMovements_BankMovementId",
                table: "InvoiceOut",
                column: "BankMovementId",
                principalTable: "BankMovements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_BankMovements_BankMovementId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_BankMovements_BankMovementId",
                table: "InvoiceOut");

            migrationBuilder.DropTable(
                name: "BankMovements");

            migrationBuilder.DropTable(
                name: "AccountingTypes");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceOut_BankMovementId",
                table: "InvoiceOut");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceIn_BankMovementId",
                table: "InvoiceIn");

            migrationBuilder.DropColumn(
                name: "BankMovementId",
                table: "InvoiceOut");

            migrationBuilder.DropColumn(
                name: "BankMovementId",
                table: "InvoiceIn");
        }
    }
}
