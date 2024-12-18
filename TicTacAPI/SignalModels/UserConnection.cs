namespace TicTacAPI.SignalModels
{
    public class UserConnection
    {
        public string Username { get; set; } = String.Empty;
        public required string GameId { get; set; }
    }
}
