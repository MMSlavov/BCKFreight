namespace BCKFreightTMS.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class NoteCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "NoteInfo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteInfo_CurrencyId",
                table: "NoteInfo",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteInfo_Currencies_CurrencyId",
                table: "NoteInfo",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteInfo_Currencies_CurrencyId",
                table: "NoteInfo");

            migrationBuilder.DropIndex(
                name: "IX_NoteInfo_CurrencyId",
                table: "NoteInfo");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "NoteInfo");
        }
    }
}
