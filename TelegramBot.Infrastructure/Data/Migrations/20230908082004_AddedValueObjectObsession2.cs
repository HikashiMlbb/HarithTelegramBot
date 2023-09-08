using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBot.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedValueObjectObsession2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account",
                table: "Members");
        }
    }
}
