using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaturaTakip.Migrations
{
    public partial class messageRefactroed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FKUserId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FKUserId",
                table: "Messages",
                column: "FKUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_FKUserId",
                table: "Messages",
                column: "FKUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_FKUserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_FKUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FKUserId",
                table: "Messages");
        }
    }
}
