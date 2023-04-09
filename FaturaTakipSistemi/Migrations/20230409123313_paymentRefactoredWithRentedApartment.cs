using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaturaTakip.Migrations
{
    public partial class paymentRefactoredWithRentedApartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RentedApartments_FKApartmentId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "FKApartmentId",
                table: "Payments",
                newName: "FKRentedApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_FKApartmentId",
                table: "Payments",
                newName: "IX_Payments_FKRentedApartmentId");

            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RentedApartments_FKRentedApartmentId",
                table: "Payments",
                column: "FKRentedApartmentId",
                principalTable: "RentedApartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RentedApartments_FKRentedApartmentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "FKRentedApartmentId",
                table: "Payments",
                newName: "FKApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_FKRentedApartmentId",
                table: "Payments",
                newName: "IX_Payments_FKApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RentedApartments_FKApartmentId",
                table: "Payments",
                column: "FKApartmentId",
                principalTable: "RentedApartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
