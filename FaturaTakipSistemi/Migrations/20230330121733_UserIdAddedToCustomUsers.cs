using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaturaTakip.Migrations
{
    public partial class UserIdAddedToCustomUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FK_UserId",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FK_UserId",
                table: "Landlords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FK_UserId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "FK_UserId",
                table: "Landlords");
        }
    }
}
