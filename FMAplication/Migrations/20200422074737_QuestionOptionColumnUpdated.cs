using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class QuestionOptionColumnUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_OptionTitle",
                table: "QuestionOptions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_OptionTitle",
                table: "QuestionOptions",
                column: "OptionTitle",
                unique: true);
        }
    }
}
