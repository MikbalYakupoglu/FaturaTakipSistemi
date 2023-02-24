using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaturaTakip.Migrations
{
    public partial class ApartmentMessage_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Landlords_FKLandlordId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Tenants_FKTenantId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DoorNumber",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "FKTenantId",
                table: "Messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FKLandlordId",
                table: "Messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FKApartmentId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FKApartmentId",
                table: "Messages",
                column: "FKApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Apartments_FKApartmentId",
                table: "Messages",
                column: "FKApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Landlords_FKLandlordId",
                table: "Messages",
                column: "FKLandlordId",
                principalTable: "Landlords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Tenants_FKTenantId",
                table: "Messages",
                column: "FKTenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Apartments_FKApartmentId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Landlords_FKLandlordId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Tenants_FKTenantId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_FKApartmentId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FKApartmentId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "FKTenantId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FKLandlordId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Block",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoorNumber",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Landlords_FKLandlordId",
                table: "Messages",
                column: "FKLandlordId",
                principalTable: "Landlords",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Tenants_FKTenantId",
                table: "Messages",
                column: "FKTenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
