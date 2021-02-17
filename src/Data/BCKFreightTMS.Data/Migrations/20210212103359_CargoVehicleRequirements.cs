namespace BCKFreightTMS.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CargoVehicleRequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VehicleRequirements",
                table: "Cargos",
                type: "nvarchar(MAX)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleRequirements",
                table: "Cargos");
        }
    }
}
