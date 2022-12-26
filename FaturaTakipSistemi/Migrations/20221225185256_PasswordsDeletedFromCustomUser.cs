using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaturaTakip.Migrations
{
    public partial class PasswordsDeletedFromCustomUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Landlords");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Landlords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Tenants",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Tenants",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Landlords",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Landlords",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
