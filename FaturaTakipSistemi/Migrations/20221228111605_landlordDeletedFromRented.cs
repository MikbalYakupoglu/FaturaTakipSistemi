using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaturaTakip.Migrations
{
    public partial class landlordDeletedFromRented : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentedApartments_Landlords_FKLandlordId",
                table: "RentedApartments");

            migrationBuilder.DropIndex(
                name: "IX_RentedApartments_FKLandlordId",
                table: "RentedApartments");

            migrationBuilder.DropColumn(
                name: "FKLandlordId",
                table: "RentedApartments");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_FKLandlordId",
                table: "Apartments",
                column: "FKLandlordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_Landlords_FKLandlordId",
                table: "Apartments",
                column: "FKLandlordId",
                principalTable: "Landlords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_Landlords_FKLandlordId",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_FKLandlordId",
                table: "Apartments");

            migrationBuilder.AddColumn<int>(
                name: "FKLandlordId",
                table: "RentedApartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RentedApartments_FKLandlordId",
                table: "RentedApartments",
                column: "FKLandlordId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentedApartments_Landlords_FKLandlordId",
                table: "RentedApartments",
                column: "FKLandlordId",
                principalTable: "Landlords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
