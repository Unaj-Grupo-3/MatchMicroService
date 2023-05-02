using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Like",
                table: "UserMatches");

            migrationBuilder.RenameColumn(
                name: "UserSecundaryId",
                table: "UserMatches",
                newName: "User2");

            migrationBuilder.RenameColumn(
                name: "UserMainId",
                table: "UserMatches",
                newName: "User1");

            migrationBuilder.AddColumn<int>(
                name: "LikeUser1",
                table: "UserMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikeUser2",
                table: "UserMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserMatches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeUser1",
                table: "UserMatches");

            migrationBuilder.DropColumn(
                name: "LikeUser2",
                table: "UserMatches");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserMatches");

            migrationBuilder.RenameColumn(
                name: "User2",
                table: "UserMatches",
                newName: "UserSecundaryId");

            migrationBuilder.RenameColumn(
                name: "User1",
                table: "UserMatches",
                newName: "UserMainId");

            migrationBuilder.AddColumn<bool>(
                name: "Like",
                table: "UserMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
