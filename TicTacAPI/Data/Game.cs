namespace TicTacAPI.Data
{
    public class Game
    {
        public int Id { get; set; }
        public string[] GameState { get; set; } = ["", "", "", "", "", "", "", "", ""];
        public string Player1 { get; set; } = "";
        public string Player2 { get; set; } = "";
        public string Turn { get; set; } = "";
    }
}