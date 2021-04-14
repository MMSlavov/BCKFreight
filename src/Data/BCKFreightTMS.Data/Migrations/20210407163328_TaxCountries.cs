namespace BCKFreightTMS.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class TaxCountries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaxCountryId",
                table: "OrderActions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderActions_TaxCountryId",
                table: "OrderActions",
                column: "TaxCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderActions_TaxCountries_TaxCountryId",
                table: "OrderActions",
                column: "TaxCountryId",
                principalTable: "TaxCountries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderActions_TaxCountries_TaxCountryId",
                table: "OrderActions");

            migrationBuilder.DropIndex(
                name: "IX_OrderActions_TaxCountryId",
                table: "OrderActions");

            migrationBuilder.DropColumn(
                name: "TaxCountryId",
                table: "OrderActions");
        }
    }
}
