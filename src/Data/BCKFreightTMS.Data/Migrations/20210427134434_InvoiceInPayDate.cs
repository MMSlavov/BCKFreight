using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BCKFreightTMS.Data.Migrations
{
    public partial class InvoiceInPayDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PayDate",
                table: "InvoiceIn",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayDate",
                table: "InvoiceIn");
        }
    }
}
