using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCKFreightTMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteInfoAndDecimalPrecisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_BankDetails_BankDetailsId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_BankMovements_BankMovementId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_InvoiceIn_InvoiceNoteInId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_InvoiceStatuses_StatusId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_NoteInfo_InvoiceNoteId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIn_VATReasons_VATReasonId",
                table: "InvoiceIn");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_BankDetails_BankDetailsId",
                table: "InvoiceOut");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_BankMovements_BankMovementId",
                table: "InvoiceOut");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_InvoiceOut_InvoiceNoteOutId",
                table: "InvoiceOut");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_InvoiceStatuses_StatusId",
                table: "InvoiceOut");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_NoteInfo_InvoiceNoteId",
                table: "InvoiceOut");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOut_VATReasons_VATReasonId",
                table: "InvoiceOut");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTos_InvoiceIn_InvoiceInId",
                table: "OrderTos");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTos_InvoiceOut_InvoiceOutId",
                table: "OrderTos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceOut",
                table: "InvoiceOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceIn",
                table: "InvoiceIn");

            migrationBuilder.RenameTable(
                name: "InvoiceOut",
                newName: "InvoiceOuts");

            migrationBuilder.RenameTable(
                name: "InvoiceIn",
                newName: "InvoiceIns");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOut_VATReasonId",
                table: "InvoiceOuts",
                newName: "IX_InvoiceOuts_VATReasonId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOut_StatusId",
                table: "InvoiceOuts",
                newName: "IX_InvoiceOuts_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOut_IsDeleted",
                table: "InvoiceOuts",
                newName: "IX_InvoiceOuts_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOut_InvoiceNoteOutId",
                table: "InvoiceOuts",
                newName: "IX_InvoiceOuts_InvoiceNoteOutId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOut_InvoiceNoteId",
                table: "InvoiceOuts",
                newName: "IX_InvoiceOuts_InvoiceNoteId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOut_BankMovementId",
                table: "InvoiceOuts",
                newName: "IX_InvoiceOuts_BankMovementId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOut_BankDetailsId",
                table: "InvoiceOuts",
                newName: "IX_InvoiceOuts_BankDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_VATReasonId",
                table: "InvoiceIns",
                newName: "IX_InvoiceIns_VATReasonId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_StatusId",
                table: "InvoiceIns",
                newName: "IX_InvoiceIns_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_IsDeleted",
                table: "InvoiceIns",
                newName: "IX_InvoiceIns_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_InvoiceNoteInId",
                table: "InvoiceIns",
                newName: "IX_InvoiceIns_InvoiceNoteInId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_InvoiceNoteId",
                table: "InvoiceIns",
                newName: "IX_InvoiceIns_InvoiceNoteId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_BankMovementId",
                table: "InvoiceIns",
                newName: "IX_InvoiceIns_BankMovementId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIn_BankDetailsId",
                table: "InvoiceIns",
                newName: "IX_InvoiceIns_BankDetailsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceOuts",
                table: "InvoiceOuts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceIns",
                table: "InvoiceIns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIns_BankDetails_BankDetailsId",
                table: "InvoiceIns",
                column: "BankDetailsId",
                principalTable: "BankDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIns_BankMovements_BankMovementId",
                table: "InvoiceIns",
                column: "BankMovementId",
                principalTable: "BankMovements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIns_InvoiceIns_InvoiceNoteInId",
                table: "InvoiceIns",
                column: "InvoiceNoteInId",
                principalTable: "InvoiceIns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIns_InvoiceStatuses_StatusId",
                table: "InvoiceIns",
                column: "StatusId",
                principalTable: "InvoiceStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIns_NoteInfo_InvoiceNoteId",
                table: "InvoiceIns",
                column: "InvoiceNoteId",
                principalTable: "NoteInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIns_VATReasons_VATReasonId",
                table: "InvoiceIns",
                column: "VATReasonId",
                principalTable: "VATReasons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOuts_BankDetails_BankDetailsId",
                table: "InvoiceOuts",
                column: "BankDetailsId",
                principalTable: "BankDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOuts_BankMovements_BankMovementId",
                table: "InvoiceOuts",
                column: "BankMovementId",
                principalTable: "BankMovements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOuts_InvoiceOuts_InvoiceNoteOutId",
                table: "InvoiceOuts",
                column: "InvoiceNoteOutId",
                principalTable: "InvoiceOuts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOuts_InvoiceStatuses_StatusId",
                table: "InvoiceOuts",
                column: "StatusId",
                principalTable: "InvoiceStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOuts_NoteInfo_InvoiceNoteId",
                table: "InvoiceOuts",
                column: "InvoiceNoteId",
                principalTable: "NoteInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOuts_VATReasons_VATReasonId",
                table: "InvoiceOuts",
                column: "VATReasonId",
                principalTable: "VATReasons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTos_InvoiceIns_InvoiceInId",
                table: "OrderTos",
                column: "InvoiceInId",
                principalTable: "InvoiceIns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTos_InvoiceOuts_InvoiceOutId",
                table: "OrderTos",
                column: "InvoiceOutId",
                principalTable: "InvoiceOuts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIns_BankDetails_BankDetailsId",
                table: "InvoiceIns");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIns_BankMovements_BankMovementId",
                table: "InvoiceIns");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIns_InvoiceIns_InvoiceNoteInId",
                table: "InvoiceIns");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIns_InvoiceStatuses_StatusId",
                table: "InvoiceIns");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIns_NoteInfo_InvoiceNoteId",
                table: "InvoiceIns");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceIns_VATReasons_VATReasonId",
                table: "InvoiceIns");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOuts_BankDetails_BankDetailsId",
                table: "InvoiceOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOuts_BankMovements_BankMovementId",
                table: "InvoiceOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOuts_InvoiceOuts_InvoiceNoteOutId",
                table: "InvoiceOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOuts_InvoiceStatuses_StatusId",
                table: "InvoiceOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOuts_NoteInfo_InvoiceNoteId",
                table: "InvoiceOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceOuts_VATReasons_VATReasonId",
                table: "InvoiceOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTos_InvoiceIns_InvoiceInId",
                table: "OrderTos");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTos_InvoiceOuts_InvoiceOutId",
                table: "OrderTos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceOuts",
                table: "InvoiceOuts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceIns",
                table: "InvoiceIns");

            migrationBuilder.RenameTable(
                name: "InvoiceOuts",
                newName: "InvoiceOut");

            migrationBuilder.RenameTable(
                name: "InvoiceIns",
                newName: "InvoiceIn");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOuts_VATReasonId",
                table: "InvoiceOut",
                newName: "IX_InvoiceOut_VATReasonId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOuts_StatusId",
                table: "InvoiceOut",
                newName: "IX_InvoiceOut_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOuts_IsDeleted",
                table: "InvoiceOut",
                newName: "IX_InvoiceOut_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOuts_InvoiceNoteOutId",
                table: "InvoiceOut",
                newName: "IX_InvoiceOut_InvoiceNoteOutId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOuts_InvoiceNoteId",
                table: "InvoiceOut",
                newName: "IX_InvoiceOut_InvoiceNoteId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOuts_BankMovementId",
                table: "InvoiceOut",
                newName: "IX_InvoiceOut_BankMovementId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceOuts_BankDetailsId",
                table: "InvoiceOut",
                newName: "IX_InvoiceOut_BankDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIns_VATReasonId",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_VATReasonId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIns_StatusId",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIns_IsDeleted",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIns_InvoiceNoteInId",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_InvoiceNoteInId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIns_InvoiceNoteId",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_InvoiceNoteId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIns_BankMovementId",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_BankMovementId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceIns_BankDetailsId",
                table: "InvoiceIn",
                newName: "IX_InvoiceIn_BankDetailsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceOut",
                table: "InvoiceOut",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceIn",
                table: "InvoiceIn",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_BankDetails_BankDetailsId",
                table: "InvoiceIn",
                column: "BankDetailsId",
                principalTable: "BankDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_BankMovements_BankMovementId",
                table: "InvoiceIn",
                column: "BankMovementId",
                principalTable: "BankMovements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_InvoiceIn_InvoiceNoteInId",
                table: "InvoiceIn",
                column: "InvoiceNoteInId",
                principalTable: "InvoiceIn",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_InvoiceStatuses_StatusId",
                table: "InvoiceIn",
                column: "StatusId",
                principalTable: "InvoiceStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_NoteInfo_InvoiceNoteId",
                table: "InvoiceIn",
                column: "InvoiceNoteId",
                principalTable: "NoteInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceIn_VATReasons_VATReasonId",
                table: "InvoiceIn",
                column: "VATReasonId",
                principalTable: "VATReasons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_BankDetails_BankDetailsId",
                table: "InvoiceOut",
                column: "BankDetailsId",
                principalTable: "BankDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_BankMovements_BankMovementId",
                table: "InvoiceOut",
                column: "BankMovementId",
                principalTable: "BankMovements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_InvoiceOut_InvoiceNoteOutId",
                table: "InvoiceOut",
                column: "InvoiceNoteOutId",
                principalTable: "InvoiceOut",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_InvoiceStatuses_StatusId",
                table: "InvoiceOut",
                column: "StatusId",
                principalTable: "InvoiceStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_NoteInfo_InvoiceNoteId",
                table: "InvoiceOut",
                column: "InvoiceNoteId",
                principalTable: "NoteInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceOut_VATReasons_VATReasonId",
                table: "InvoiceOut",
                column: "VATReasonId",
                principalTable: "VATReasons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTos_InvoiceIn_InvoiceInId",
                table: "OrderTos",
                column: "InvoiceInId",
                principalTable: "InvoiceIn",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTos_InvoiceOut_InvoiceOutId",
                table: "OrderTos",
                column: "InvoiceOutId",
                principalTable: "InvoiceOut",
                principalColumn: "Id");
        }
    }
}
