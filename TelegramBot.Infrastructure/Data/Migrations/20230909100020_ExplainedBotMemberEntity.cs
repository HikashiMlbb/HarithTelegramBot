using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBot.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExplainedBotMemberEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Members",
                newName: "Current Level");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Members",
                newName: "First name");

            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "Members",
                newName: "Current experience");

            migrationBuilder.AlterColumn<long>(
                name: "TelegramId",
                table: "Members",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<long>(
                name: "ChatId",
                table: "Members",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "Current Level",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "First name",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<float>(
                name: "Current experience",
                table: "Members",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "REAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "First name",
                table: "Members",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Current experience",
                table: "Members",
                newName: "Experience");

            migrationBuilder.RenameColumn(
                name: "Current Level",
                table: "Members",
                newName: "Level");

            migrationBuilder.AlterColumn<long>(
                name: "TelegramId",
                table: "Members",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ChatId",
                table: "Members",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Members",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<float>(
                name: "Experience",
                table: "Members",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 0f);

            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "Members",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }
    }
}
