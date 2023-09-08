using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBot.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AcceptedNewPrimitiveObsession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Account_TelegramId",
                table: "Members",
                newName: "TelegramId");

            migrationBuilder.RenameColumn(
                name: "Account_ChatId",
                table: "Members",
                newName: "ChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TelegramId",
                table: "Members",
                newName: "Account_TelegramId");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Members",
                newName: "Account_ChatId");
        }
    }
}
