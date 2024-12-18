using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TicTacAPI.Data;

namespace TicTacAPI.SignalModels
{
    public class GameHub : Hub
    {
        private AppDbContext _context;

        public GameHub(AppDbContext context) =>  _context = context;

        public async Task JoinGame(UserConnection conn)
        {
            await Clients.All.SendAsync("RecieveMessage", "admin", $"{conn.Username} has joined");
        }

        public async Task JoinSpecificGame(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.GameId);
            var id = Int32.Parse(conn.GameId);
            var game = await _context.Games.FindAsync(id);

            //// New Game
            if (game == null)
            {
                game = new Game();
                game.Id = id;
                game.Player1 = conn.Username;
                game.Turn = game.Player1;
                _context.Games.Add(game);
                await _context.SaveChangesAsync();
                await Clients.Group(conn.GameId).SendAsync("JoinSpecificGame", game);
            }
            else // Game already exists, adds second player
            {
                game.Player2 = conn.Username;
                _context.Entry(game).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    await Clients.Group(conn.GameId).SendAsync("JoinSpecificGame", game);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }

        public async Task UpdateGameState(string id, string[] gameState)
        {
            var game = await _context.Games.FindAsync(Int32.Parse(id));
            if (game == null)
            {
                await Clients.Group(id).SendAsync("UpdateGameState", null, "");
            }
            else
            {
                game.GameState = gameState;
                // Checks if the player just won on that move
                var winner = CheckWinCondition(gameState) ? game.Turn : "";
                if (game.Turn == game.Player1)
                {
                    game.Turn = game.Player2;
                }
                else
                {
                    game.Turn = game.Player1;
                }
                _context.Entry(game).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    await Clients.Group(id).SendAsync("UpdateGameState", gameState, winner);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }
        private bool CheckWinCondition(string[] gameState)
        {
            return false;
        }
    }
}
