using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaturaTakip.Migrations
{
    public partial class messageRefactoredWithSender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_FKUserId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "FKUserId",
                table: "Messages",
                newName: "FkSenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_FKUserId",
                table: "Messages",
                newName: "IX_Messages_FkSenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_FkSenderId",
                table: "Messages",
                column: "FkSenderId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_FkSenderId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "FkSenderId",
                table: "Messages",
                newName: "FKUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_FkSenderId",
                table: "Messages",
                newName: "IX_Messages_FKUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_FKUserId",
                table: "Messages",
                column: "FKUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
