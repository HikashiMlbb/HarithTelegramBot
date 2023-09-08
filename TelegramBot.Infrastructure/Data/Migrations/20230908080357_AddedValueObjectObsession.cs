using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBot.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedValueObjectObsession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "TelegramId",
                table: "Members");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "Members",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TelegramId",
                table: "Members",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
