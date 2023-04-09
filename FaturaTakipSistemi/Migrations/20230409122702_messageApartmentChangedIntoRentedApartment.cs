using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaturaTakip.Migrations
{
    public partial class messageApartmentChangedIntoRentedApartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debts_Apartments_FKApartmentId",
                table: "Debts");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Apartments_FKApartmentId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Apartments_FKApartmentId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "FKApartmentId",
                table: "Messages",
                newName: "FKRentedApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_FKApartmentId",
                table: "Messages",
                newName: "IX_Messages_FKRentedApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_RentedApartments_FKApartmentId",
                table: "Debts",
                column: "FKApartmentId",
                principalTable: "RentedApartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_RentedApartments_FKRentedApartmentId",
                table: "Messages",
                column: "FKRentedApartmentId",
                principalTable: "RentedApartments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RentedApartments_FKApartmentId",
                table: "Payments",
                column: "FKApartmentId",
                principalTable: "RentedApartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debts_RentedApartments_FKApartmentId",
                table: "Debts");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_RentedApartments_FKRentedApartmentId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RentedApartments_FKApartmentId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "FKRentedApartmentId",
                table: "Messages",
                newName: "FKApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_FKRentedApartmentId",
                table: "Messages",
                newName: "IX_Messages_FKApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_Apartments_FKApartmentId",
                table: "Debts",
                column: "FKApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Apartments_FKApartmentId",
                table: "Messages",
                column: "FKApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Apartments_FKApartmentId",
                table: "Payments",
                column: "FKApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
