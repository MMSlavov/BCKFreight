using Microsoft.EntityFrameworkCore.Migrations;

namespace BCKFreightTMS.Data.Migrations
{
    public partial class AddPropertyToBaseModelAdminId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "VehicleTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "VehicleLoadingBodies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "TaxCountries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "PersonRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "People",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "OrderTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "OrderTos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "OrderStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "OrderFroms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "OrderActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Comunicators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "CompanyContacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "CompanyAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "CargoTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Cargos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "BankDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "ActionTypes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "VehicleLoadingBodies");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "TaxCountries");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "PersonRoles");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "OrderTypes");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "OrderTos");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "OrderFroms");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "OrderActions");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Comunicators");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "CompanyContacts");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "CompanyAddresses");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "CargoTypes");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Cargos");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "BankDetails");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "ActionTypes");
        }
    }
}
