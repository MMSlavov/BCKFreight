using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BCKFreightTMS.Data.Migrations
{
    public partial class OrderFromReceiveDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiveDate",
                table: "OrderFroms",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiveDate",
                table: "OrderFroms");
        }
    }
}
