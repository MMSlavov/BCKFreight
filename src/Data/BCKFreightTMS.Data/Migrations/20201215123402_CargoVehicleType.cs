using Microsoft.EntityFrameworkCore.Migrations;

namespace BCKFreightTMS.Data.Migrations
{
    public partial class CargoVehicleType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "VehicleTypes",
                newName: "Details");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Vehicles",
                type: "nvarchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "Cargos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_VehicleTypeId",
                table: "Cargos",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cargos_VehicleTypes_VehicleTypeId",
                table: "Cargos",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cargos_VehicleTypes_VehicleTypeId",
                table: "Cargos");

            migrationBuilder.DropIndex(
                name: "IX_Cargos_VehicleTypeId",
                table: "Cargos");

            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "Cargos");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "VehicleTypes",
                newName: "MyProperty");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Vehicles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldNullable: true);
        }
    }
}
