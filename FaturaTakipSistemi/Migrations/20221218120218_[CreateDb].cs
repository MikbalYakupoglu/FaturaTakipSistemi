using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaturaTakip.Migrations
{
    public partial class CreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Floor = table.Column<short>(type: "smallint", nullable: false),
                    DoorNumber = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Block = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Landlords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GovermentId = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    YearOfBirth = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landlords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LisencePlate = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GovermentId = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    YearOfBirth = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Debts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dues = table.Column<int>(type: "int", nullable: false),
                    Bill = table.Column<int>(type: "int", nullable: false),
                    FKApartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Debts_Apartments_FKApartmentId",
                        column: x => x.FKApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FKTenantId = table.Column<int>(type: "int", nullable: false),
                    FKLandlordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Landlords_FKLandlordId",
                        column: x => x.FKLandlordId,
                        principalTable: "Landlords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Tenants_FKTenantId",
                        column: x => x.FKTenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    FKTenantId = table.Column<int>(type: "int", nullable: false),
                    FKApartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Apartments_FKApartmentId",
                        column: x => x.FKApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Tenants_FKTenantId",
                        column: x => x.FKTenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentedApartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    FKTenantId = table.Column<int>(type: "int", nullable: false),
                    FKApartmentId = table.Column<int>(type: "int", nullable: false),
                    FKLandlordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentedApartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentedApartments_Apartments_FKApartmentId",
                        column: x => x.FKApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentedApartments_Landlords_FKLandlordId",
                        column: x => x.FKLandlordId,
                        principalTable: "Landlords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentedApartments_Tenants_FKTenantId",
                        column: x => x.FKTenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Debts_FKApartmentId",
                table: "Debts",
                column: "FKApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FKLandlordId",
                table: "Messages",
                column: "FKLandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FKTenantId",
                table: "Messages",
                column: "FKTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FKApartmentId",
                table: "Payments",
                column: "FKApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FKTenantId",
                table: "Payments",
                column: "FKTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RentedApartments_FKApartmentId",
                table: "RentedApartments",
                column: "FKApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RentedApartments_FKLandlordId",
                table: "RentedApartments",
                column: "FKLandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_RentedApartments_FKTenantId",
                table: "RentedApartments",
                column: "FKTenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Debts");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "RentedApartments");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "Landlords");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
