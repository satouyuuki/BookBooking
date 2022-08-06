using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBooking.Migrations
{
    public partial class ChangeBookHistoryPropertyAttribute2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BookHistory_BookId",
                table: "BookHistory",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookHistory_UserId",
                table: "BookHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookHistory_Books_BookId",
                table: "BookHistory",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookHistory_Users_UserId",
                table: "BookHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookHistory_Books_BookId",
                table: "BookHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_BookHistory_Users_UserId",
                table: "BookHistory");

            migrationBuilder.DropIndex(
                name: "IX_BookHistory_BookId",
                table: "BookHistory");

            migrationBuilder.DropIndex(
                name: "IX_BookHistory_UserId",
                table: "BookHistory");
        }
    }
}
