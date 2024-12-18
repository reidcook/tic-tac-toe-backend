namespace TicTacAPI.SignalModels
{
    public class UserConnection
    {
        public int Id { get; set; }
        public string Username { get; set; } = String.Empty;
        public string GameId { get; set; } = String.Empty;
        public string? ConnectionId {  get; set; }
    }
}
