using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class SPWisePOSMLedgerUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpWisePosmLedgers_POSMProducts_PosmProductId",
                table: "SpWisePosmLedgers");

            migrationBuilder.DropColumn(
                name: "PosmProuctId",
                table: "SpWisePosmLedgers");

            migrationBuilder.AlterColumn<int>(
                name: "PosmProductId",
                table: "SpWisePosmLedgers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SpWisePosmLedgers_POSMProducts_PosmProductId",
                table: "SpWisePosmLedgers",
                column: "PosmProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpWisePosmLedgers_POSMProducts_PosmProductId",
                table: "SpWisePosmLedgers");

            migrationBuilder.AlterColumn<int>(
                name: "PosmProductId",
                table: "SpWisePosmLedgers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PosmProuctId",
                table: "SpWisePosmLedgers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_SpWisePosmLedgers_POSMProducts_PosmProductId",
                table: "SpWisePosmLedgers",
                column: "PosmProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
