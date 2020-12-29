namespace BCKFreightTMS.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class FixCargoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Cargos",
                newName: "Details");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "Cargos",
                newName: "MyProperty");
        }
    }
}
