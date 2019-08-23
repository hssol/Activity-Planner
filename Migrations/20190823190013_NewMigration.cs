using Microsoft.EntityFrameworkCore.Migrations;

namespace BeltExam.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Occurances_CreatorId",
                table: "Occurances",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Occurances_Users_CreatorId",
                table: "Occurances",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Occurances_Users_CreatorId",
                table: "Occurances");

            migrationBuilder.DropIndex(
                name: "IX_Occurances_CreatorId",
                table: "Occurances");
        }
    }
}
