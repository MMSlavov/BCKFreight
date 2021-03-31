using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BCKFreightTMS.Data.Migrations
{
    public partial class InvoiceOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIns_BankDetails_BankDetailsId",
                table: "InvoiceIns");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIns_InvoiceStatuses_StatusId",
                table: "InvoiceIns");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTos_InvoiceIns_InvoiceInId",
                table: "OrderTos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceIns",
                table: "InvoiceIns");

            migrationBuilder.RenameTable(
                name: "InvoiceIns",
                newName: "InvoiceIn");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIns_StatusId",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIns_IsDeleted",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIns_BankDetailsId",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_BankDetailsId");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceOutId",
                table: "OrderTos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceIn",
                table: "InvoiceIn",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InvoiceOut",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankDetailsId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDays = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceOut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceOut_BankDetails_BankDetailsId",
                        column: x => x.BankDetailsId,
                        principalTable: "BankDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceOut_InvoiceStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "InvoiceStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderTos_InvoiceOutId",
                table: "OrderTos",
                column: "InvoiceOutId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceOut_BankDetailsId",
                table: "InvoiceOut",
                column: "BankDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceOut_IsDeleted",
                table: "InvoiceOut",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceOut_StatusId",
                table: "InvoiceOut",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_BankDetails_BankDetailsId",
                table: "InvoiceIn",
                column: "BankDetailsId",
                principalTable: "BankDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_InvoiceStatuses_StatusId",
                table: "InvoiceIn",
                column: "StatusId",
                principalTable: "InvoiceStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTos_InvoiceIn_InvoiceInId",
                table: "OrderTos",
                column: "InvoiceInId",
                principalTable: "InvoiceIn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTos_InvoiceOut_InvoiceOutId",
                table: "OrderTos",
                column: "InvoiceOutId",
                principalTable: "InvoiceOut",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_BankDetails_BankDetailsId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_InvoiceStatuses_StatusId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTos_InvoiceIn_InvoiceInId",
                table: "OrderTos");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTos_InvoiceOut_InvoiceOutId",
                table: "OrderTos");

            migrationBuilder.DropTable(
                name: "InvoiceOut");

            migrationBuilder.DropIndex(
                name: "IX_OrderTos_InvoiceOutId",
                table: "OrderTos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceIn",
                table: "InvoiceIn");

            migrationBuilder.DropColumn(
                name: "InvoiceOutId",
                table: "OrderTos");

            migrationBuilder.RenameTable(
                name: "InvoiceIn",
                newName: "InvoiceIns");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_StatusId",
                table: "InvoiceIns",
                newName: "IX_InvoiceIns_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_IsDeleted",
                table: "InvoiceIns",
                newName: "IX_InvoiceIns_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_BankDetailsId",
                table: "InvoiceIns",
                newName: "IX_InvoiceIns_BankDetailsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceIns",
                table: "InvoiceIns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIns_BankDetails_BankDetailsId",
                table: "InvoiceIns",
                column: "BankDetailsId",
                principalTable: "BankDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIns_InvoiceStatuses_StatusId",
                table: "InvoiceIns",
                column: "StatusId",
                principalTable: "InvoiceStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTos_InvoiceIns_InvoiceInId",
                table: "OrderTos",
                column: "InvoiceInId",
                principalTable: "InvoiceIns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
