namespace BCKFreightTMS.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CurrencyDocumentationActionNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "OrderTos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentationId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "OrderFroms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoNotes",
                table: "OrderActions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "OrderActions",
                type: "nvarchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaxCurrencyId",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documentations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CMR = table.Column<bool>(type: "bit", nullable: false),
                    BillOfLading = table.Column<bool>(type: "bit", nullable: false),
                    AOA = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryNote = table.Column<bool>(type: "bit", nullable: false),
                    PackingList = table.Column<bool>(type: "bit", nullable: false),
                    ListItems = table.Column<bool>(type: "bit", nullable: false),
                    Invoice = table.Column<bool>(type: "bit", nullable: false),
                    BillOfGoods = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderTos_CurrencyId",
                table: "OrderTos",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DocumentationId",
                table: "Orders",
                column: "DocumentationId",
                unique: true,
                filter: "[DocumentationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFroms_CurrencyId",
                table: "OrderFroms",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TaxCurrencyId",
                table: "Companies",
                column: "TaxCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_IsDeleted",
                table: "Currency",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Documentations_IsDeleted",
                table: "Documentations",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Currency_TaxCurrencyId",
                table: "Companies",
                column: "TaxCurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderFroms_Currency_CurrencyId",
                table: "OrderFroms",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Documentations_DocumentationId",
                table: "Orders",
                column: "DocumentationId",
                principalTable: "Documentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTos_Currency_CurrencyId",
                table: "OrderTos",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Currency_TaxCurrencyId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderFroms_Currency_CurrencyId",
                table: "OrderFroms");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Documentations_DocumentationId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTos_Currency_CurrencyId",
                table: "OrderTos");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Documentations");

            migrationBuilder.DropIndex(
                name: "IX_OrderTos_CurrencyId",
                table: "OrderTos");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DocumentationId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderFroms_CurrencyId",
                table: "OrderFroms");

            migrationBuilder.DropIndex(
                name: "IX_Companies_TaxCurrencyId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "OrderTos");

            migrationBuilder.DropColumn(
                name: "DocumentationId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "OrderFroms");

            migrationBuilder.DropColumn(
                name: "NoNotes",
                table: "OrderActions");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "OrderActions");

            migrationBuilder.DropColumn(
                name: "TaxCurrencyId",
                table: "Companies");
        }
    }
}
