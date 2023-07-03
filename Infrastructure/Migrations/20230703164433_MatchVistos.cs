using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MatchVistos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "View",
                table: "Matches",
                newName: "View2");

            migrationBuilder.AddColumn<bool>(
                name: "View1",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "View1",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "View2",
                table: "Matches",
                newName: "View");
        }
    }
}
