using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacAPI.Migrations
{
    /// <inheritdoc />
    public partial class GameVariablenameupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "turn",
                table: "Games",
                newName: "Turn");

            migrationBuilder.RenameColumn(
                name: "player2",
                table: "Games",
                newName: "Player2");

            migrationBuilder.RenameColumn(
                name: "player1",
                table: "Games",
                newName: "Player1");

            migrationBuilder.RenameColumn(
                name: "gameState",
                table: "Games",
                newName: "GameState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Turn",
                table: "Games",
                newName: "turn");

            migrationBuilder.RenameColumn(
                name: "Player2",
                table: "Games",
                newName: "player2");

            migrationBuilder.RenameColumn(
                name: "Player1",
                table: "Games",
                newName: "player1");

            migrationBuilder.RenameColumn(
                name: "GameState",
                table: "Games",
                newName: "gameState");
        }
    }
}
