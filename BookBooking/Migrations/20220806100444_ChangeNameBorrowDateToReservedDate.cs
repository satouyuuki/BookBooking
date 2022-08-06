using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBooking.Migrations
{
    public partial class ChangeNameBorrowDateToReservedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BorrowedDate",
                table: "BookHistory",
                newName: "ReservedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReservedDate",
                table: "BookHistory",
                newName: "BorrowedDate");
        }
    }
}
