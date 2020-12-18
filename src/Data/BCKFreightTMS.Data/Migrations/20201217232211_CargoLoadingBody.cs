using Microsoft.EntityFrameworkCore.Migrations;

namespace BCKFreightTMS.Data.Migrations
{
    public partial class CargoLoadingBody : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoadingBodyId",
                table: "Cargos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_LoadingBodyId",
                table: "Cargos",
                column: "LoadingBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cargos_VehicleLoadingBodies_LoadingBodyId",
                table: "Cargos",
                column: "LoadingBodyId",
                principalTable: "VehicleLoadingBodies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cargos_VehicleLoadingBodies_LoadingBodyId",
                table: "Cargos");

            migrationBuilder.DropIndex(
                name: "IX_Cargos_LoadingBodyId",
                table: "Cargos");

            migrationBuilder.DropColumn(
                name: "LoadingBodyId",
                table: "Cargos");
        }
    }
}
