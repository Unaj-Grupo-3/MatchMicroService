using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LikeIntState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dates_MatchId",
                table: "Dates");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Dates");

            migrationBuilder.AlterColumn<int>(
                name: "LikeUser2",
                table: "UserMatches",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "LikeUser1",
                table: "UserMatches",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_Dates_MatchId",
                table: "Dates",
                column: "MatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dates_MatchId",
                table: "Dates");

            migrationBuilder.AlterColumn<bool>(
                name: "LikeUser2",
                table: "UserMatches",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "LikeUser1",
                table: "UserMatches",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Dates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Dates_MatchId",
                table: "Dates",
                column: "MatchId",
                unique: true);
        }
    }
}
