using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

            // Checking if a game with this ID exists
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
                // Setting the connection ID, and saving to DB
                conn.ConnectionId = Context.ConnectionId;
                _context.Connections.Add(conn);
                await _context.SaveChangesAsync();
                await Clients.Group(conn.GameId).SendAsync("JoinSpecificGame", game);
            }
            else // Game already exists, adds second player
            {
                // Game is full
                if (game.Player1 != "" & game.Player2 != "")
                {
                    await Clients.Group(conn.GameId).SendAsync("JoinSpecificGame", null);
                }
                else
                {
                    // Setting the connection ID, and saving to DB
                    conn.ConnectionId = Context.ConnectionId;
                    _context.Connections.Add(conn);

                    // Sets the player that needs to be filled
                    if (game.Player1 == "")
                    {
                        game.Player1 = conn.Username;
                    }
                    else
                    {
                        game.Player2 = conn.Username;
                    }

                    // Game already exists, and a player left during their turn
                    if (game.Turn == "")
                    {
                        game.Turn = conn.Username;
                    }
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
                    await Clients.Group(id).SendAsync("UpdateGameState", game, winner);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }
        private bool CheckWinCondition(string[] gameState)
        {
            for(int i = 0; i < 3; i++)
            {
                // Checking veritcal
                if (gameState[i] != "" & gameState[i] == gameState[i + 3] & gameState[i] == gameState[i + 6])
                {
                    return true;
                }
                // Checking Horizontal
                if (gameState[i * 3] != "" & gameState[i * 3] == gameState[(i * 3) + 1] & gameState[i * 3] == gameState[(i * 3) + 2])
                {
                    return true;
                }
            }
            return false;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Find connection to get access to gameId
            var connection = _context.Connections.Where(c => c.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (connection != null)
            {
                // Find game the connection was in
                var game = _context.Games.Where(g => g.Id == Int32.Parse(connection.GameId)).FirstOrDefault();
                if (game != null)
                {
                    // This is the first player disconnecting
                    if (game.Player1 != "" & game.Player2 != "")
                    {
                        // Remove player from the game
                        if (game.Player1 == connection.Username)
                        {
                            game.Player1 = "";
                            game.Turn = "";
                        }
                        else if (game.Player2 == connection.Username)
                        {
                            game.Player2 = "";
                            game.Turn = "";
                        }
                        // Update remaining player
                        await Clients.Group(connection.GameId).SendAsync("UpdateGameState", game, "");
                    }
                    // Both players are disconnected. Delete the game
                    else
                    {
                        _context.Remove(game);
                    }
                }
                _context.Remove(connection);
                await _context.SaveChangesAsync();
            }
            Trace.WriteLine(Context.ConnectionId + " - disconnected");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
